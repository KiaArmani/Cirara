using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cirara.Models.Data;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using static System.String;
using Branch = Cirara.Models.Data.Branch;

namespace Cirara.Services
{
    public class GitService
    {
        private readonly IConfiguration _configuration;

        public GitService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void CreateRepository(string remoteUrl)
        {
            var repoPath = GetRepositoryLocalPath(remoteUrl);

            var options = new CloneOptions
            {
                CredentialsProvider = (url, user, cred) => GetUsernamePasswordCredentials()
            };

            Repository.Clone(remoteUrl, repoPath, options);
        }

        public void UpdateRepository(string remoteUrl)
        {
            var repoPath = GetRepositoryLocalPath(remoteUrl);
            if (!Directory.Exists(repoPath))
                CreateRepository(remoteUrl);

            var repo = new Repository(repoPath);
            var creds = GetUsernamePasswordCredentials();

            Credentials CredHandler(string url, string user, SupportedCredentialTypes cred)
            {
                return creds;
            }

            var fetchOpts = new FetchOptions {CredentialsProvider = CredHandler};

            var remote = repo.Network.Remotes["origin"];
            var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);

            var logMessage = Empty;
            Commands.Fetch(repo, remote.Name, refSpecs, fetchOpts, logMessage);
        }

        private UsernamePasswordCredentials GetUsernamePasswordCredentials(string credentialNamespace = "Motion")
        {
            return new UsernamePasswordCredentials
            {
                Username = _configuration[$"{credentialNamespace}:User"],
                Password = _configuration[$"{credentialNamespace}:Pass"]
            };
        }

        public Repo GetRepository(string repoName)
        {
            if (GetRepositoryFromDisk(repoName, out var repository)) 
                return null;

            // Create return object
            var repo = new Repo
            {
                Name = repository.Network.Remotes.First().Name,
                Branches = new List<Branch>()
            };

            // Browse remote branches and collect commits
            foreach (var b in repository.Branches.Where(b => b.IsRemote))
                repo.Branches.Add(GetBranchFromGitBranch(b));

            // Return repo data
            return repo;
        }

        public Branch GetBranch(string repoName, string branchName)
        {
            if (GetGitBranch(repoName, branchName, out var gitBranch)) 
                return null;
            return gitBranch == null ? null : GetBranchFromGitBranch(gitBranch);
        }

        private bool GetGitBranch(string repoName, string branchName, out LibGit2Sharp.Branch gitBranch)
        {
            gitBranch = null;
            if (GetRepositoryFromDisk(repoName, out var repository))
                return true;
            gitBranch = repository.Branches.Single(x => x.FriendlyName.Equals($"origin/{branchName}"));
            return false;
        }

        public SlimCommit GetSlimCommit(string repoName, string branchName, string commitHash)
        {
            GetGitBranch(repoName, branchName, out var gitBranch);
            var gitCommit = gitBranch.Commits.Single(x => x.Sha.Equals(commitHash));
            return new SlimCommit
            {
                CommitDate = gitCommit.Committer.When,
                Commiter = gitCommit.Committer.Name,
                CommitMessage = gitCommit.Message
            };
        }

        private static Branch GetBranchFromGitBranch(LibGit2Sharp.Branch gitBranch)
        {
            return new Branch
            {
                Commits = gitBranch.Commits.Select(x => new SlimCommit
                {
                    CommitDate = x.Committer.When,
                    Commiter = x.Committer.Name,
                    CommitMessage = x.Message
                }).ToList(),
                Name = gitBranch.FriendlyName.Replace("origin/", "")
            };
        }

        private bool GetRepositoryFromDisk(string repoName, out Repository repository)
        {
            // Get Repository URL from Secrets
            repository = null;
            var repoUrl = _configuration[$"{repoName}:Url"];
            if (IsNullOrEmpty(repoUrl))
                return false;

            // Try to locate Repo on disk
            var repoPath = GetRepositoryLocalPath(repoUrl);
            if (!Directory.Exists(repoPath))
                CreateRepository(repoUrl);

            // Get repo from disk
            repository = GetRepositoryInternal(repoUrl);
            return false;
        }

        public Repository GetRepositoryInternal(string remoteUrl)
        {
            var repoPath = GetRepositoryLocalPath(remoteUrl);
            if (!Directory.Exists(repoPath))
                CreateRepository(remoteUrl);

            var repo = new Repository(repoPath);
            return repo;
        }

        public string GetRepositoryLocalPath(string remoteUrl)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var uri = new Uri(remoteUrl);
            var uriWithoutScheme = uri.Host + uri.PathAndQuery + uri.Fragment;

            var remoteUrlFriendlyName = uriWithoutScheme.Replace("\\", "-");
            return Path.Combine(basePath, remoteUrlFriendlyName);
        }
    }
}
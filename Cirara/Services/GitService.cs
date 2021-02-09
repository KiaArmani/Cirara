using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cirara.Models.Data;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
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
                CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials
                {
                    Username = _configuration["Motion:User"],
                    Password = _configuration["Motion:Pass"]
                },
            };

            Repository.Clone(remoteUrl, repoPath, options);
        }

        public void UpdateRepository(string remoteUrl)
        {
            var repoPath = GetRepositoryLocalPath(remoteUrl);
            if (!Directory.Exists(repoPath))
                CreateRepository(remoteUrl);

            var repo = new Repository(repoPath);
            var creds = new UsernamePasswordCredentials()
            {
                Username = _configuration["Motion:User"],
                Password = _configuration["Motion:Pass"]
            };

            Credentials CredHandler(string url, string user, SupportedCredentialTypes cred) => creds;
            var fetchOpts = new FetchOptions { CredentialsProvider = CredHandler };

            var remote = repo.Network.Remotes["origin"];
            var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);

            var logMessage = String.Empty;
            Commands.Fetch(repo, remote.Name, refSpecs, fetchOpts, logMessage);            
        }

        public Repo GetRepository(string remoteUrl)
        {
            var repoPath = GetRepositoryLocalPath(remoteUrl);
            if (!Directory.Exists(repoPath))
                CreateRepository(remoteUrl);
            var repository = GetRepositoryInternal(remoteUrl);

            var repo = new Repo
            {
                Name = repository.Network.Remotes.First().Name,
                Branches = new List<Branch>()
            };

            foreach (var b in repository.Branches.Where(b => b.IsRemote))
            {
                repo.Branches.Add(new Branch
                {
                    Commits = b.Commits.Select(x => new SlimCommit
                    {
                        CommitDate = x.Committer.When,
                        Commiter = x.Committer.Name,
                        CommitMessage = x.Message
                    }).ToList(),
                    Name = b.FriendlyName.Replace("origin/", "")
                });
            }

            return repo;
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
            Uri uri = new Uri(remoteUrl);
            string uriWithoutScheme = uri.Host + uri.PathAndQuery + uri.Fragment;

            var remoteUrlFriendlyName = uriWithoutScheme.Replace("\\", "-");
            return Path.Combine(basePath, remoteUrlFriendlyName);
        }
    }
}
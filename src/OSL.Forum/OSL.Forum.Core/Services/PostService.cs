﻿using System;
using System.Collections.Generic;
using System.Linq;
using OSL.Forum.Core.Enums;
using OSL.Forum.Core.Repositories;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;
using OSL.Forum.Core.UnitOfWorks;

namespace OSL.Forum.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService()
        {
            _postRepository = new PostRepository();
        }

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public BO.Post GetPost(long postId)
        {
            if (postId == 0)
                throw new ArgumentException("Post Id is required.");

            var postEntity = _postRepository.GetById(postId);

            if (postEntity == null)
                return null;

            var post = new BO.Post()
            {
                Id = postEntity.Id,
                Name = postEntity.Name,
                Description = postEntity.Description,
                CreationDate = postEntity.CreationDate,
                ModificationDate = postEntity.ModificationDate,
                TopicId = postEntity.TopicId,
                ApplicationUserId = postEntity.ApplicationUserId,
                Status = postEntity.Status,
                Topic = new BO.Topic()
                {
                    Id = postEntity.Topic.Id,
                    Name = postEntity.Topic.Name,
                    ForumId = postEntity.Topic.ForumId,
                    Forum = new BO.Forum()
                    {
                        Id = postEntity.Topic.Forum.Id,
                        Name = postEntity.Topic.Forum.Name,
                        CategoryId = postEntity.Topic.Forum.CategoryId,
                        Category = new BO.Category()
                        {
                            Id = postEntity.Topic.Forum.Category.Id,
                            Name = postEntity.Topic.Forum.Category.Name
                        }
                    }
                }
            };

            return post;
        }

        public void EditPost(BO.Post post)
        {
            if (post is null)
                throw new ArgumentNullException(nameof(post));

            var postEntity = _postRepository.GetById(post.Id);

            if (postEntity is null)
                throw new InvalidOperationException("Post is not found.");

            postEntity.Name = post.Name;
            postEntity.Description = post.Description;
            postEntity.ModificationDate = post.ModificationDate;
            postEntity.Status = Status.Pending.ToString();

            _postRepository.Save();
        }

        public void DeletePost(long postId)
        {
            if (postId == 0)
                throw new ArgumentException("Post Id is required.");

            _postRepository.RemoveById(postId);
            _postRepository.Save();
        }

        public void CreatePost(BO.Post post)
        {
            if (post is null)
                throw new ArgumentNullException(nameof(post));

            var postEntity = new EO.Post()
            {
                Id = post.Id,
                Name = post.Name,
                Description = post.Description,
                CreationDate = post.CreationDate,
                ModificationDate = post.ModificationDate,
                TopicId = post.TopicId,
                ApplicationUserId = post.ApplicationUserId,
                Status = post.Status
            };

            _postRepository.Add(postEntity);
            _postRepository.Save();
        }

        public List<BO.Post> GetMyPosts(int pagerCurrentPage, int pagerPageSize, string userId)
        {
            var postEntity = _postRepository.LoadUserPosts(userId, pagerCurrentPage, pagerPageSize, false);

            if (postEntity == null)
                return null;

            var posts = new List<BO.Post>();

            foreach (var post in postEntity)
            {
                posts.Add(new BO.Post()
                {
                    Id = post.Id,
                    Name = post.Name,
                    Description = post.Description,
                    CreationDate = post.CreationDate,
                    ModificationDate = post.ModificationDate,
                    TopicId = post.TopicId,
                    ApplicationUserId = post.ApplicationUserId,
                    Status = post.Status
                });
            }

            return posts;
        }

        public int GetPendingPostCount()
        {
            var pendingPostCount = _postRepository.LoadByStatus(Status.Pending.ToString()).Count;

            return pendingPostCount == 0 ? 0 : pendingPostCount;
        }

        public List<BO.Post> PendingPosts(int pagerCurrentPage, int pagerPageSize)
        {
            var postEntity = _postRepository.LoadPendingPosts(Status.Pending.ToString(), pagerCurrentPage, pagerPageSize, false);

            if (postEntity == null)
                return null;

            var posts = new List<BO.Post>();

            foreach (var post in postEntity)
            {
                posts.Add(new BO.Post()
                {
                    Id = post.Id,
                    Name = post.Name,
                    Description = post.Description,
                    CreationDate = post.CreationDate,
                    ModificationDate = post.ModificationDate,
                    TopicId = post.TopicId,
                    ApplicationUserId = post.ApplicationUserId,
                    Status = post.Status,
                    Topic = new BO.Topic()
                    {
                        Id = post.Topic.Id,
                        Name = post.Topic.Name,
                        CreationDate = post.Topic.CreationDate,
                        ModificationDate = post.Topic.ModificationDate,
                        ApplicationUserId = post.Topic.ApplicationUserId,
                        Status = post.Topic.Status,
                        Forum = new BO.Forum()
                        {
                            Id = post.Topic.Forum.Id,
                            Name = post.Topic.Forum.Name,
                            CreationDate = post.Topic.Forum.CreationDate,
                            ModificationDate = post.Topic.Forum.ModificationDate,
                            ApplicationUserId = post.Topic.Forum.ApplicationUserId,
                            Category = new BO.Category()
                            {
                                Id = post.Topic.Forum.Category.Id,
                                Name = post.Topic.Forum.Category.Name
                            }
                        }
                    }
                });
            }

            return posts;
        }

        public void UpdatePostStatus(long postId, string status)
        {
            if (postId == 0 && string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Post Id or status missing.");

            var postEntity = _postRepository.GetById(postId);

            if (postEntity == null)
                throw new InvalidOperationException("Post is not found.");

            postEntity.Status = status;

            _postRepository.Save();
        }

        public int UserPostCount(string applicationUserId)
        {
            if (string.IsNullOrWhiteSpace(applicationUserId))
                throw new ArgumentException("User id is missing.");

            var userPendingPostCount = _postRepository.LoadByUserId(applicationUserId).Count;

            return userPendingPostCount == 0 ? 0 : userPendingPostCount;
        }
    }
}

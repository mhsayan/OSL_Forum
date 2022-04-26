﻿using System;
using System.Collections.Generic;
using System.Linq;
using OSL.Forum.Core.Enums;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;
using OSL.Forum.Core.UnitOfWorks;

namespace OSL.Forum.Core.Services
{
    public class PostService : IPostService
    {
        private static PostService _postService;
        private readonly CoreUnitOfWork _unitOfWork;

        private PostService()
        {
            _unitOfWork = CoreUnitOfWork.Create();
        }

        public static PostService Create()
        {
            //if (_postService == null)
            //{
            //    _postService = new PostService();
            //}

            return new PostService();
        }

        public BO.Post GetPost(long postId)
        {
            if (postId == 0)
                throw new ArgumentException("Post Id is required.");

            var postEntity = _unitOfWork.Posts.Get(c => c.Id == postId, "").FirstOrDefault();

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
                Status = postEntity.Status
            };

            return post;
        }

        public void EditPost(BO.Post post)
        {
            if (post is null)
                throw new ArgumentNullException(nameof(post));

            var postEntity = _unitOfWork.Posts.GetById(post.Id);

            if (postEntity is null)
                throw new InvalidOperationException("Post is not found.");

            postEntity.Name = post.Name;
            postEntity.Description = post.Description;
            postEntity.ModificationDate = post.ModificationDate;
            postEntity.Status = Status.Pending.ToString();

            _unitOfWork.Save();
        }

        public void DeletePost(long postId)
        {
            if (postId == 0)
                throw new ArgumentException("Post Id is required.");

            _unitOfWork.Posts.Remove(postId);
            _unitOfWork.Save();
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

            _unitOfWork.Posts.Add(postEntity);
            _unitOfWork.Save();
        }

        public List<BO.Post> GetMyPosts(int pagerCurrentPage, int pagerPageSize, string userId)
        {
            var postEntity = _unitOfWork.Posts.Get(p => p.ApplicationUserId == userId, q => q.OrderByDescending(c => c.ModificationDate), "", pagerCurrentPage, pagerPageSize, false);

            if (postEntity.data == null)
                return null;

            var posts = new List<BO.Post>();

            foreach (var post in postEntity.data)
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
            var pendingPostCount = _unitOfWork.Posts.Get(p => p.Status == Status.Pending.ToString(), "").Count;

            return pendingPostCount == 0 ? 0 : pendingPostCount;
        }

        public List<BO.Post> PendingPosts(int pagerCurrentPage, int pagerPageSize)
        {
            var postEntity = _unitOfWork.Posts.Get(p => p.Status == Status.Pending.ToString(), q => q.OrderBy(c => c.ModificationDate), "", pagerCurrentPage, pagerPageSize, false);

            if (postEntity.data == null)
                return null;

            var posts = new List<BO.Post>();

            foreach (var post in postEntity.data)
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

        public void UpdatePostStatus(long postId, string status)
        {
            if (postId == 0 && string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Post Id or status missing.");

            var postEntity = _unitOfWork.Posts.GetById(postId);

            if (postEntity == null)
                throw new InvalidOperationException("Post is not found.");

            postEntity.Status = status;

            _unitOfWork.Save();
        }

        public int UserPostCount(string applicationUserId)
        {
            if (string.IsNullOrWhiteSpace(applicationUserId))
                throw new ArgumentException("User id is missing.");

            var userPendingPostCount = _unitOfWork.Posts.Get(p => p.ApplicationUserId == applicationUserId, "").Count;

            return userPendingPostCount == 0 ? 0 : userPendingPostCount;
        }
    }
}

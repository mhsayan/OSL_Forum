using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OSL.Forum.Core.Enums;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;
using OSL.Forum.Core.UnitOfWorks;

namespace OSL.Forum.Core.Services
{
    public class PostService : IPostService
    {
        private readonly ICoreUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public PostService(ICoreUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BO.Post GetPost(long postId)
        {
            if (postId == null)
                throw new ArgumentNullException(nameof(postId));

            var postEntity = _unitOfWork.Posts.Get(c => c.Id == postId, "").FirstOrDefault();

            if (postEntity == null)
                return null;

            var post = _mapper.Map<BO.Post>(postEntity);

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
            if (postId == null)
                throw new ArgumentNullException(nameof(postId));

            _unitOfWork.Posts.Remove(postId);
            _unitOfWork.Save();
        }

        public void CreatePost(BO.Post post)
        {
            if (post is null)
                throw new ArgumentNullException(nameof(post));

            var postEntity = _mapper.Map<EO.Post>(post);

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
                posts.Add(_mapper.Map<BO.Post>(post));
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
                posts.Add(_mapper.Map<BO.Post>(post));
            }

            return posts;
        }

        public void UpdatePostStatus(long postId, string status)
        {
            if (postId == null && string.IsNullOrWhiteSpace(status))
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

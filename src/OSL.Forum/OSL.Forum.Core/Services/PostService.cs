using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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

        public BO.Post GetPost(Guid postId)
        {
            if (postId == Guid.Empty)
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

            _unitOfWork.Save();
        }

        public void DeletePost(Guid postId)
        {
            if (postId == Guid.Empty)
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

        public List<BO.Post> GetMyPosts(string userId)
        {
            var postEntity = _unitOfWork.Posts.Get(p => p.ApplicationUserId == userId, "");

            if (postEntity == null)
                return null;

            var posts = new List<BO.Post>();

            foreach (var post in postEntity)
            {
                posts.Add(_mapper.Map<BO.Post>(post));
            }

            return posts;
        }
    }
}

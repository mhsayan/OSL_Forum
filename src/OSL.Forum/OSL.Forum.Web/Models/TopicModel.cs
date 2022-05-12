using System;
using System.Collections.Generic;
using OSL.Forum.Common.Utilities;
using BO = OSL.Forum.Entities.BusinessObjects;

namespace OSL.Forum.Web.Models
{
    public class TopicModel
    {
        public BO.Forum Forum { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsFavorite { get; set; }
        public IList<BO.Topic> Topics { get; set; }
        public IList<string> UserRoles { get; set; }
        public Pager Pager { get; set; }
        private ApplicationUser ApplicationUser { get; set; }
        public BO.Topic Topic { get; set; }
        public DateTime Time { get; set; }

        public BO.Post PostBuilder(BO.Post topicPost)
        {
            var post = new BO.Post()
            {
                Id = topicPost.Id,
                Name = topicPost.Name,
                TopicId = topicPost.TopicId,
                ApplicationUserId = topicPost.ApplicationUserId,
                Owner = topicPost.Owner,
                OwnerName = topicPost.OwnerName,
                Description = topicPost.Description,
                Status = topicPost.Status,
                CreationDate = topicPost.CreationDate,
                ModificationDate = topicPost.ModificationDate,
                Topic = topicPost.Topic
            };

            return post;
        }
    }
}
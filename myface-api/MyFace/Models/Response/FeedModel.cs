using System;
using System.Collections.Generic;
using System.Linq;
using MyFace.Models.Database;
using MyFace.Models.Request;
using System.Web;

namespace MyFace.Models.Response
{
    public class FeedInteractionModel : InteractionResponse
    {
        public FeedInteractionModel(Interaction interaction) : base(interaction)
        {
            User = new FeedUserModel(interaction.User);
        }
        
        public FeedUserModel User { get; }
    }
    
    public class FeedUserModel : UserResponse
    {
        public FeedUserModel(User user) : base(user)
        { }
    }
    
    public class FeedPostModel : PostResponse
    {
        public FeedPostModel(Post post) : base(post)
        {
            PostedBy = new FeedUserModel(post.User);
            Likes = post.Interactions
                .Where(i => i.Type == InteractionType.LIKE)
                .Select(i => new FeedInteractionModel(i));
            Dislikes = post.Interactions
                .Where(i => i.Type == InteractionType.DISLIKE)
                .Select(i => new FeedInteractionModel(i));
        }

        public FeedUserModel PostedBy { get; }
        public IEnumerable<FeedInteractionModel> Likes { get; }
        public IEnumerable<FeedInteractionModel> Dislikes { get; }
    }

    public class FeedModel : ListResponse<FeedPostModel>
    {
        private FeedModel(SearchRequest search, IEnumerable<FeedPostModel> items, int totalNumberOfItems) 
            : base(search, items, totalNumberOfItems, "feed") { }

        public static FeedModel Create(SearchRequest searchRequest, IEnumerable<Post> posts, int totalNumberOfItems)
        {
            var feedModels = posts.Select(p => new FeedPostModel(p));
            return new FeedModel(searchRequest, feedModels, totalNumberOfItems);
        }
    }
}
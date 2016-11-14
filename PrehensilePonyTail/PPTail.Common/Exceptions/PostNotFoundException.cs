using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Exceptions
{
    public class PostNotFoundException: System.Exception
    {
        public Guid PostId { get; set; }

        public PostNotFoundException(Guid postId)
            : base($"Unable to locate post with Id={postId.ToString()}")
        {
            this.PostId = postId;
        }
    }
}

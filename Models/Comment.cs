using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Models
{
    public class Comment
    {
        public string CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        // Другие свойства комментария
    }
}

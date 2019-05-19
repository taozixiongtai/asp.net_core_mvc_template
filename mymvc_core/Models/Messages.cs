using mymvc_core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mymvc_core.Models
{
    public partial class Messages
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "留言")]
        public string Message { get; set; }
        [Required]
        [Display(Name = "日期")]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "你的ID或者联系方式")]
        public string Submission { get; set; }
        [Required]
        [Display(Name = "留言保存方式")]
        public MessageStatus Isprivate { get; set; }
    }
}

//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using TraderData.Models;

//namespace Trader.Models.ContactViewModels
//{

//    public class ContactView
//    {
//        public int EnquiryId { get; set; }
//        [Display(Name = "QueryType", ResourceType = typeof(ViewRes.SharedStrings))]
//        public QueryType queryType { get; set; }
//        [Required(ErrorMessageResourceType = typeof(ViewRes.SharedStrings),
//                      ErrorMessageResourceName = "EnquiryError")]
//        [Display(Name = "Message", ResourceType = typeof(ViewRes.SharedStrings))]
//        public string message { get; set; }
//        [Display(Name = "Timestamp", ResourceType = typeof(ViewRes.SharedStrings))]
//        public DateTime ts { get; set; }
//    }
//    public class EnquiryListView
//    {
//        public int EnquiryId { get; set; }
//        public ApplicationUser user { get; set; }
//        [Display(Name = "QueryType", ResourceType = typeof(ViewRes.SharedStrings))]
//        public QueryType queryType { get; set; }
//        public string message { get; set; }
//        [Display(Name = "Timestamp", ResourceType = typeof(ViewRes.SharedStrings))]
//        public DateTime ts { get; set; }
//    }
//    public class ContactListView
//    {
//        [Display(Name = "QueryType", ResourceType = typeof(ViewRes.SharedStrings))]
//        public QueryType queryType { get; set; }
//        public List<Message> messages { get; set; }
//    }
//}

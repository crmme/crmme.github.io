using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace crmme.Model
{
    public class MailResponse
    {
        [JsonPropertyName("@odata.context")]
        public Uri OdataContext { get; set; }
        [JsonPropertyName("@odata.nextlink")]
        public string OdataNextLink { get; set; }
        public MailMessage[] Value { get; set; }
    }

    public partial class MailMessage
    {

        public string OdataEtag { get; set; }
        public string Id { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
        public DateTimeOffset LastModifiedDateTime { get; set; }
        public string ChangeKey { get; set; }

        public DateTimeOffset ReceivedDateTime { get; set; }
        public DateTimeOffset SentDateTime { get; set; }

        public string InternetMessageId { get; set; }
        public string Subject { get; set; }
        public string BodyPreview { get; set; }

        public string ParentFolderId { get; set; }
        public string ConversationId { get; set; }
        public string ConversationIndex { get; set; }

        public Uri WebLink { get; set; }

        public Body Body { get; set; }
        public From Sender { get; set; }
        public From From { get; set; }
        public From[] ToRecipients { get; set; }
        public EmailAddress[] CcRecipients { get; set; }
        public EmailAddress[] BccRecipients { get; set; }
        public EmailAddress[] ReplyTo { get; set; }
    }

    public partial class Body
    {

        public string Content { get; set; }
    }



    public partial class From
    {
        public EmailAddress EmailAddress { get; set; }
    }

   
}

using System;
using BioEngine.Data.Core;

namespace BioEngine.Data.Social.Commands
{
    public class PublishFacebookLinkCommand : QueryBase<string>
    {
        public Uri Link { get; }

        public PublishFacebookLinkCommand(Uri link)
        {
            Link = link;
        }
    }
}
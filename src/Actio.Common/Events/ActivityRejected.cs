using System;

namespace Actio.Common.Events
{
    public class ActivityRejected : IRejectedEvent
    {
        public Guid UserId { get; }
        public string Reason {get;}
        public string Code {get;}

        protected ActivityRejected(){}

        public ActivityRejected(Guid userId, string reason, string code)
        {
            UserId = userId;
            Reason = reason;
            Code = code;
        }
    }
}
using HealthCare.Core.AddRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Specifications.NotificationSpecification
{
    public class NotificationSpec : Specifications<Notification>
    {
        public NotificationSpec(string Email) : base(N => (N.ReceiverEmail == Email) && (N.Status == NotificationStatus.Pending))
        {
            IsPaginationEnable = false;
        }
    }
}

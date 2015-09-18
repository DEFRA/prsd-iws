namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain.MovementOperationReceipt;
    using Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TestableMovementOperationReceipt : MovementOperationReceipt
    {
        public new DateTime Date
        {
            get { return base.Date; }
            set { ObjectInstantiator<MovementOperationReceipt>.SetProperty(x => x.Date, value, this); }
        }
    }
}

namespace EA.Iws.RequestHandlers.Tests.Unit.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using Requests.WasteCodes;
    using Xunit;

    public class BaseSetCodesTests
    {
        [Fact]
        public void CreateWithBothCodesAndNotApplicable_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new TestSetCodes(Guid.Empty, new[] { Guid.Empty }, true));
        }

        [Fact]
        public void CreateWithCodesOnly_DoesNotThrow()
        {
            var result = new TestSetCodes(Guid.Empty, new[] { Guid.Empty }, false);

            Assert.Equal(Guid.Empty, result.Id);
        }

        [Fact]
        public void CreateNotApplicableTrueAndNullEnumerable_DoesNotThrow()
        {
            var result = new TestSetCodes(Guid.Empty, null, true);

            Assert.Equal(Guid.Empty, result.Id);
        }

        [Fact]
        public void CreateNotApplicableTrueAndEmptyEnumerable_DoesNotThrow()
        {
            var result = new TestSetCodes(Guid.Empty, new Guid[0], true);

            Assert.Equal(Guid.Empty, result.Id);
        }

        private class TestSetCodes : BaseSetCodes
        {
            public TestSetCodes(Guid id, IEnumerable<Guid> codes, bool isNotApplicable) 
                : base(id, codes, isNotApplicable)
            {
            }
        }
    }
}

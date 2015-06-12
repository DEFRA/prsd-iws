namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Xunit;

    public class UserTests
    {
        [Fact]
        public void CanCreateUser()
        {
            var user = new User("id", "first", "last", "123", "email@address.com");

            Assert.NotNull(user);
        }

        [Fact]
        public void IdCannotBeNull()
        {
            Action createUser = () => new User(null, "first", "last", "123", "email@address.com");

            Assert.Throws<ArgumentNullException>("id", createUser);
        }

        [Fact]
        public void IdCannotBeEmpty()
        {
            Action createUser = () => new User(string.Empty, "first", "last", "123", "email@address.com");

            Assert.Throws<ArgumentException>("id", createUser);
        }

        [Fact]
        public void FirstNameCannotBeNull()
        {
            Action createUser = () => new User("id", null, "last", "123", "email@address.com");

            Assert.Throws<ArgumentNullException>("firstName", createUser);
        }

        [Fact]
        public void FirstNameCannotBeEmpty()
        {
            Action createUser = () => new User("id", string.Empty, "last", "123", "email@address.com");

            Assert.Throws<ArgumentException>("firstName", createUser);
        }

        [Fact]
        public void SurnameCannotBeNull()
        {
            Action createUser = () => new User("id", "first", null, "123", "email@address.com");

            Assert.Throws<ArgumentNullException>("surname", createUser);
        }

        [Fact]
        public void SurnameCannotBeEmpty()
        {
            Action createUser = () => new User("id", "first", string.Empty, "123", "email@address.com");

            Assert.Throws<ArgumentException>("surname", createUser);
        }

        [Fact]
        public void PhoneNumberCannotBeNull()
        {
            Action createUser = () => new User("id", "first", "last", null, "email@address.com");

            Assert.Throws<ArgumentNullException>("phoneNumber", createUser);
        }

        [Fact]
        public void PhoneNumberCannotBeEmpty()
        {
            Action createUser = () => new User("id", "first", "last", string.Empty, "email@address.com");

            Assert.Throws<ArgumentException>("phoneNumber", createUser);
        }

        [Fact]
        public void EmailCannotBeNull()
        {
            Action createUser = () => new User("id", "first", "last", "123", null);

            Assert.Throws<ArgumentNullException>("email", createUser);
        }

        [Fact]
        public void EmailCannotBeEmpty()
        {
            Action createUser = () => new User("id", "first", "last", "123", string.Empty);

            Assert.Throws<ArgumentException>("email", createUser);
        }

        [Fact]
        public void CanLinkToOrganisation()
        {
            var user = new User("id", "first", "last", "123", "email@address.com");
            var address = new Address("building", "address1", "address2", "town", "region", "postcode", "country");
            var org = new Organisation("name", address, "type", "123");

            user.LinkToOrganisation(org);

            Assert.NotNull(user.Organisation);
        }

        [Fact]
        public void CannotLinkToSecondOrganisation()
        {
            var user = new User("id", "first", "last", "123", "email@address.com");
            var address = new Address("building", "address1", "address2", "town", "region", "postcode", "country");
            var org = new Organisation("name", address, "type", "123");

            user.LinkToOrganisation(org);

            var secondAddress = new Address("building2", "address12", "address22", "town2", "region2", "postcode2", "country2");
            var secondOrg = new Organisation("name2", secondAddress, "type2", "1232");

            Action linkToOrganisation = () => user.LinkToOrganisation(secondOrg);

            Assert.Throws<InvalidOperationException>(linkToOrganisation);
        }

        [Fact]
        public void LinkedOrganisationCannotBeNull()
        {
            var user = new User("id", "first", "last", "123", "email@address.com");

            Action linkToOrganisation = () => user.LinkToOrganisation(null);

            Assert.Throws<ArgumentNullException>("organisation", linkToOrganisation);
        }
    }
}
namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Domain;

    public class UserFactory
    {
        public static User Create(Guid id, string firstName, string lastName, string phoneNumber, string email)
        {
            var user = ObjectInstantiator<User>.CreateNew();

            ObjectInstantiator<User>.SetProperty(u => u.Id, id.ToString(), user);
            ObjectInstantiator<User>.SetProperty(u => u.FirstName, firstName, user);
            ObjectInstantiator<User>.SetProperty(u => u.Surname, lastName, user);
            ObjectInstantiator<User>.SetProperty(u => u.PhoneNumber, phoneNumber, user);
            ObjectInstantiator<User>.SetProperty(u => u.Email, email, user);
            ObjectInstantiator<User>.SetProperty(u => u.UserName, email, user);

            return user;
        }
    }
}
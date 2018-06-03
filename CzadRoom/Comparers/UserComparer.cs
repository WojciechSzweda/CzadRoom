using CzadRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Comparers
{
    public class UserComparer : IEqualityComparer<User> {
        public bool Equals(User x, User y) {
            if (ReferenceEquals(x, y))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return x.ID == y.ID && x.Username == y.Username;
        }

        public int GetHashCode(User user) {
            if (ReferenceEquals(user, null))
                return 0;

            return user.ID.GetHashCode() ^ user.Username.GetHashCode();
        }
    }
}

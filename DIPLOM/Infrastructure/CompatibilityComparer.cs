using DIPLOM.Model;
using System;
using System.Collections.Generic;

namespace DIPLOM.Infrastructure
{
    public class CompatibilityComparer : IEqualityComparer<Compatibility>
    {
        public bool Equals(Compatibility x, Compatibility y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Compatibility compatibility)
        {
            if (Object.ReferenceEquals(compatibility, null)) return 0;

            int hashCompatibilityName = compatibility.Name == null ? 0 : compatibility.Name.GetHashCode();

            int hashCompatibilityCode = compatibility.Id.GetHashCode();

            return hashCompatibilityName ^ hashCompatibilityCode;
        }
    }
}

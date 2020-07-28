using System;
using Chris.Personnel.Management.Common.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chris.Personnel.Management.Common.EntityModel
{
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        protected ValueObject<T> Clone()
        {
            return Json.Clone(this);
        }

        protected int GetNaiveHashCode()
        {
            return Json.ToJson(this).GetHashCode();
        }

        protected int GetMultiHashCode(params object[] args)
        {
            var hash = 0;

            if (args[0] != null)
            {
                if (args[0] is IEnumerable && !(args[0] is string))
                {
                    var contentArgs = (args[0] as IEnumerable<object> ?? throw new InvalidOperationException()).ToArray();
                    hash = GetMultiHashCode(contentArgs);
                }
                else
                {
                    hash = args[0].GetHashCode();
                }
            }

            unchecked
            {
                for (var i = 1; i < args.Length; i++)
                {
                    if (args[i] != null)
                    {
                        if (args[i] is IEnumerable && !(args[i] is string))
                        {
                            var contentArgs = (args[i] as IEnumerable<object> ?? throw new InvalidOperationException()).ToArray();

                            if (contentArgs.Any())
                            {
                                hash = hash * 397 + (GetMultiHashCode(contentArgs) ^ (i * 397));
                            }
                            else
                            {
                                hash *= 397;
                            }
                        }
                        else
                        {
                            hash = hash * 397 + (args[i].GetHashCode() ^ (i * 397));
                        }
                    }
                    else
                    {
                        hash *= 397;
                    }
                }
            }

            return hash;
        }

        protected bool IsNaiveEqual(ValueObject<T> other)
        {
            return GetNaiveHashCode() == other.GetNaiveHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is T other))
            {
                return false;
            }

            return ValueObjectIsEqual(other);
        }

        protected abstract bool ValueObjectIsEqual(T other);

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            var hash = ValueObjectGetHashCode();

            unchecked
            {
                hash = (hash * 397) ^ GetType().GetHashCode();
            }

            return hash;
        }

        /// <summary>
        /// A good implementation of this would be to compound the membervarialbes like this
        /// int hash = a.GetHashCode();
        /// hash = (hash * 397) ^ b.GetHashCode();
        /// hash = (hash * 397) ^ c.GetHashCode();
        /// ...etc...
        /// return hash
        /// 
        /// or
        /// 
        /// return GetMultiHashCode(a, b, c, ...);
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract int ValueObjectGetHashCode();
    }
}

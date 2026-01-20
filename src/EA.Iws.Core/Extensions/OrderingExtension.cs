namespace EA.Iws.Core.Extensions
{
  using EA.Iws.Core.OperationCodes;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public static class OperationOrderingExtensions
  {
    private static bool IsInterimCode(OperationCode code)
    {
      if (code == OperationCode.R12 || code == OperationCode.R13 || code == OperationCode.D15 || code == OperationCode.D14 || code == OperationCode.D13 || code == OperationCode.D12)
      {
        return true;
      }
      return false;
    }

    public static IOrderedEnumerable<OperationCode> OrderByInterimsFirst(this IEnumerable<OperationCode> source)
    {
      if (source is null)
      {
        throw new ArgumentNullException(nameof(source));
      }
      return source
          .OrderBy(c => IsInterimCode(c) ? 0 : 1)
          .ThenBy(c => c);
    }

    public static IOrderedEnumerable<T> OrderByInterimsFirst<T>(this IEnumerable<T> source, Func<T, OperationCode> keySelector)
    {
      if (source is null)
      { 
        throw new ArgumentNullException(nameof(source)); 
      }
      if (keySelector is null)
      {
        throw new ArgumentNullException(nameof(keySelector));
      }

      return source
          .OrderBy(x => IsInterimCode(keySelector(x)) ? 0 : 1)
          .ThenBy(keySelector);
    }
  }
}
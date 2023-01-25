namespace DisprzTraining.Tests.Helpers
{
    public static class ExtensionMethod
    {
        public static bool HasValue(this Guid identity)
        {
            if (identity == Guid.Empty)
                return false;
            return true;
        }
    }
}



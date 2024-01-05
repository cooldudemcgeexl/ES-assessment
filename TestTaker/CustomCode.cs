using System.Text.RegularExpressions;

namespace TestTaker
{
    public struct VersionNum(uint? major = null, uint? minor = null, uint? patch = null, uint? build = null, uint? revision = null)
    {
        public uint? Major { get; set; } = major;
        public uint? Minor { get; set; } = minor;
        public uint? Patch { get; set; } = patch;
        public uint? Build { get; set; } = build;
        public uint? Revision { get; set; } = revision;
    }

    public static class CustomCode
    {
        public static int VersionCompare(string version1, string version2)
        {
            var firstNum = ParseVersionNum(version1);
            var secondNum = ParseVersionNum(version2);

            var majorComp = CompareVersionComponent(firstNum.Major, secondNum.Major);
            if (majorComp != 0) return majorComp;

            var minorComp = CompareVersionIfExists(firstNum.Minor, secondNum.Minor);
            if (minorComp != 0) return minorComp;

            var patchComp = CompareVersionIfExists(firstNum.Patch, secondNum.Patch);
            if (patchComp != 0) return patchComp;
            var buildComp = CompareVersionIfExists(firstNum.Build, secondNum.Build);
            if (buildComp != 0) return buildComp;
            var revisionComp = CompareVersionIfExists(firstNum.Revision, secondNum.Revision);
            if (revisionComp != 0) return revisionComp;

            return 0;
        }

        private static int CompareVersionIfExists(uint? firstNum, uint? secondNum)
        {
            // If neither have a value for this component, consider them equal
            return (firstNum.HasValue || secondNum.HasValue) ? CompareVersionComponent(firstNum, secondNum) : 0;
        }

        private static uint? ParseNullableUint(int index, MatchCollection values)
        {
            if (index >= values.Count)
            {
                return null;
            }
            return uint.Parse(values[index].Value);

        }

        private static int CompareVersionComponent(uint? lVersion, uint? rVersion)
        {
            // Need to treat absent version numbers as equivalent to 0 for equality purposes.
            if (!lVersion.HasValue)
            {
                lVersion = 0;
            }
            if (!rVersion.HasValue)
            {
                rVersion = 0;
            }

            if (lVersion.Value < rVersion.Value)
            {
                return -1;
            }
            else if (lVersion.Value == rVersion.Value)
            {
                return 0;
            }
            else
            {
                return 1;
            }

        }

        public static VersionNum ParseVersionNum(string version)
        {
            // Could definitely use a fancier regex to split into capture groups if there was more time.
            Regex versionRegex = new(@"(\d+)");
            MatchCollection matches = versionRegex.Matches(version);
            var versionNum = new VersionNum(
                major: ParseNullableUint(0, matches),
                minor: ParseNullableUint(1, matches),
                patch: ParseNullableUint(2, matches),
                build: ParseNullableUint(3, matches),
                revision: ParseNullableUint(4, matches)
                );
            return versionNum;
        }
    }
}


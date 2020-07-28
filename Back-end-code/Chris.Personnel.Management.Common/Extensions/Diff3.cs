using System;
using System.Collections.Generic;
using System.Text;

namespace Chris.Personnel.Management.Common.Extensions
{
    internal static class ArrayExtension
    {
        public static T[] SliceJS<T>(this T[] array, int startingIndex, int followingIndex)
        {
            if (followingIndex > array.Length)
                followingIndex = array.Length;

            T[] outArray = new T[followingIndex - startingIndex];

            for (var i = 0; i < outArray.Length; i++)
                outArray[i] = array[i + startingIndex];

            return outArray;
        }
    }

    public static class Diff3
    {
        public static string Merge(string textOriginal, string textLeft, string textRight, bool makeLeftWin = false)
        {
            var mergedResult = Diff.Diff3Merge(
                textLeft.Split('\n'),
                textOriginal.Split('\n'),
                textRight.Split('\n'),
                true);

            var mergedText = new StringBuilder();

            foreach (var item in mergedResult)
            {
                if (item is Diff.MergeConflictResultBlock)
                {
                    if (makeLeftWin)
                    {
                        var part = (item as Diff.MergeConflictResultBlock).LeftLines;
                        mergedText.AppendLine(string.Join("\n", part));
                    }
                    else
                    {
                        throw new Exception("Merge conflict");
                    }
                }
                else
                {
                    var part = (item as Diff.MergeOKResultBlock).ContentLines;
                    mergedText.AppendLine(string.Join("\n", part));
                }
            }

            return mergedText.ToString();
        }

        private class Diff
        {
            #region Arbitrarily-named in-between objects

            public class CandidateThing
            {
                public int File1Index { get; set; }
                public int File2Index { get; set; }
                public CandidateThing Chain { get; set; }
            }

            public class CommonOrDifferentThing
            {
                public List<string> Common { get; set; }
                public List<string> File1 { get; set; }
                public List<string> File2 { get; set; }
            }

            public class PatchDescriptionThing
            {
                internal PatchDescriptionThing() { }

                internal PatchDescriptionThing(string[] file, int offset, int length)
                {
                    Offset = offset;
                    Length = length;
                    Chunk = new List<string>(file.SliceJS(offset, offset + length));
                }

                public int Offset { get; set; }
                public int Length { get; set; }
                public List<string> Chunk { get; set; }
            }

            public class PatchResult
            {
                public PatchDescriptionThing File1 { get; set; }
                public PatchDescriptionThing File2 { get; set; }
            }

            public class ChunkReference
            {
                public int Offset { get; set; }
                public int Length { get; set; }
            }

            public class DiffSet
            {
                public ChunkReference File1 { get; set; }
                public ChunkReference File2 { get; set; }
            }

            public enum Side
            {
                Conflict = -1,
                Left = 0,
                Old = 1,
                Right = 2
            }

            public class Diff3Set : IComparable<Diff3Set>
            {
                public Side Side { get; set; }
                public int File1Offset { get; set; }
                public int File1Length { get; set; }
                public int File2Offset { get; set; }
                public int File2Length { get; set; }

                public int CompareTo(Diff3Set other)
                {
                    if (File1Offset != other.File1Offset)
                        return File1Offset.CompareTo(other.File1Offset);
                    else
                        return Side.CompareTo(other.Side);
                }
            }

            public class Patch3Set
            {
                public Side Side { get; set; }
                public int Offset { get; set; }
                public int Length { get; set; }
                public int ConflictOldOffset { get; set; }
                public int ConflictOldLength { get; set; }
                public int ConflictRightOffset { get; set; }
                public int ConflictRightLength { get; set; }
            }

            private class ConflictRegion
            {
                public int File1RegionStart { get; set; }
                public int File1RegionEnd { get; set; }
                public int File2RegionStart { get; set; }
                public int File2RegionEnd { get; set; }
            }

            #endregion

            #region Merge Result Objects

            public interface IMergeResultBlock
            {
                // amusingly, I can't figure out anything they have in Common.
            }

            public class MergeOKResultBlock : IMergeResultBlock
            {
                public string[] ContentLines { get; set; }
            }

            public class MergeConflictResultBlock : IMergeResultBlock
            {
                public string[] LeftLines { get; set; }
                public int LeftIndex { get; set; }
                public string[] OldLines { get; set; }
                public int OldIndex { get; set; }
                public string[] RightLines { get; set; }
                public int RightIndex { get; set; }
            }

            #endregion

            #region Methods

            public static CandidateThing LongestCommonSubsequence(string[] file1, string[] file2)
            {
                /* Text diff algorithm following Hunt and McIlroy 1976.
                 * J. W. Hunt and M. D. McIlroy, An algorithm for differential file
                 * comparison, Bell Telephone Laboratories CSTR #41 (1976)
                 * http://www.cs.dartmouth.edu/~doug/
                 *
                 * Expects two arrays of strings.
                 */
                Dictionary<string, List<int>> equivalenceClasses = new Dictionary<string, List<int>>();
                List<int> file2indices;
                Dictionary<int, CandidateThing> candidates = new Dictionary<int, CandidateThing>();

                candidates.Add(0, new CandidateThing
                {
                    File1Index = -1,
                    File2Index = -1,
                    Chain = null
                });

                for (int j = 0; j < file2.Length; j++)
                {
                    string line = file2[j];
                    if (equivalenceClasses.ContainsKey(line))
                        equivalenceClasses[line].Add(j);
                    else
                        equivalenceClasses.Add(line, new List<int> { j });
                }

                for (int i = 0; i < file1.Length; i++)
                {
                    string line = file1[i];
                    if (equivalenceClasses.ContainsKey(line))
                        file2indices = equivalenceClasses[line];
                    else
                        file2indices = new List<int>();

                    int r = 0;

                    //This makes it faster... but is it accurate?
                    if (candidates.Count > 100)
                    {
                        r = (int)(candidates.Count * 0.95d);
                    }

                    int s = 0;
                    CandidateThing c = candidates[0];

                    for (int jX = 0; jX < file2indices.Count; jX++)
                    {
                        int j = file2indices[jX];

                        for (s = r; s < candidates.Count; s++)
                        {
                            if ((candidates[s].File2Index < j) &&
                                ((s == candidates.Count - 1) ||
                                 (candidates[s + 1].File2Index > j)))
                                break;
                        }

                        if (s < candidates.Count)
                        {
                            var newCandidate = new CandidateThing
                            {
                                File1Index = i,
                                File2Index = j,
                                Chain = candidates[s]
                            };
                            candidates[r] = c;
                            r = s + 1;
                            c = newCandidate;
                            if (r == candidates.Count)
                            {
                                break; // no point in examining further (j)s
                            }
                        }
                    }

                    candidates[r] = c;
                }

                // At this point, we know the LCS: it's in the reverse of the
                // linked-list through .Chain of
                // candidates[candidates.Length - 1].

                return candidates[candidates.Count - 1];
            }

            private static void ProcessCommon(ref CommonOrDifferentThing common, List<CommonOrDifferentThing> result)
            {
                if (common.Common.Count > 0)
                {
                    common.Common.Reverse();
                    result.Add(common);
                    common = new CommonOrDifferentThing();
                }
            }

            public static List<CommonOrDifferentThing> DiffComm(string[] file1, string[] file2)
            {
                // We apply the LCS to build a "comm"-style picture of the
                // differences between File1 and File2.

                var result = new List<CommonOrDifferentThing>();

                int tail1 = file1.Length;
                int tail2 = file2.Length;

                CommonOrDifferentThing common = new CommonOrDifferentThing
                {
                    Common = new List<string>()
                };

                for (var candidate = Diff.LongestCommonSubsequence(file1, file2);
                     candidate != null;
                     candidate = candidate.Chain)
                {
                    CommonOrDifferentThing different = new CommonOrDifferentThing
                    {
                        File1 = new List<string>(),
                        File2 = new List<string>()
                    };

                    while (--tail1 > candidate.File1Index)
                        different.File1.Add(file1[tail1]);

                    while (--tail2 > candidate.File2Index)
                        different.File2.Add(file2[tail2]);

                    if (different.File1.Count > 0 || different.File2.Count > 0)
                    {
                        ProcessCommon(ref common, result);
                        different.File1.Reverse();
                        different.File2.Reverse();
                        result.Add(different);
                    }

                    if (tail1 >= 0)
                        common.Common.Add(file1[tail1]);
                }

                ProcessCommon(ref common, result);

                result.Reverse();
                return result;
            }

            public static List<PatchResult> DiffPatch(string[] file1, string[] file2)
            {
                // We apply the LCD to build a JSON representation of a
                // diff(1)-style patch.

                var result = new List<PatchResult>();
                var tail1 = file1.Length;
                var tail2 = file2.Length;

                for (var candidate = Diff.LongestCommonSubsequence(file1, file2);
                     candidate != null;
                     candidate = candidate.Chain)
                {
                    var mismatchLength1 = tail1 - candidate.File1Index - 1;
                    var mismatchLength2 = tail2 - candidate.File2Index - 1;
                    tail1 = candidate.File1Index;
                    tail2 = candidate.File2Index;

                    if (mismatchLength1 > 0 || mismatchLength2 > 0)
                    {
                        PatchResult thisResult = new PatchResult
                        {
                            File1 = new PatchDescriptionThing(file1,
                                                             candidate.File1Index + 1,
                                                             mismatchLength1),
                            File2 = new PatchDescriptionThing(file2,
                                                             candidate.File2Index + 1,
                                                             mismatchLength2)
                        };
                        result.Add(thisResult);
                    }
                }

                result.Reverse();
                return result;
            }

            public static List<PatchResult> StripPatch(List<PatchResult> patch)
            {
                // Takes the output of Diff.diff_patch(), and removes
                // information from it. It can still be used by patch(),
                // below, but can no longer be inverted.
                var newpatch = new List<PatchResult>();
                for (var i = 0; i < patch.Count; i++)
                {
                    var chunk = patch[i];
                    newpatch.Add(new PatchResult
                    {
                        File1 = new PatchDescriptionThing
                        {
                            Offset = chunk.File1.Offset,
                            Length = chunk.File1.Length
                        },
                        File2 = new PatchDescriptionThing
                        {
                            Chunk = chunk.File1.Chunk
                        }
                    });
                }
                return newpatch;
            }

            public static void InvertPatch(List<PatchResult> patch)
            {
                // Takes the output of Diff.diff_patch(), and inverts the
                // sense of it, so that it can be applied to File2 to give
                // File1 rather than the other way around.
                for (var i = 0; i < patch.Count; i++)
                {
                    var chunk = patch[i];
                    var tmp = chunk.File1;
                    chunk.File1 = chunk.File2;
                    chunk.File2 = tmp;
                }
            }

            private static void CopyCommon(int targetOffset, ref int commonOffset, string[] file, List<string> result)
            {
                while (commonOffset < targetOffset)
                {
                    result.Add(file[commonOffset]);
                    commonOffset++;
                }
            }

            public static List<string> Patch(string[] file, List<PatchResult> patch)
            {
                // Applies a patch to a file.
                //
                // Given File1 and File2, Diff.patch(File1, Diff.diff_patch(File1, File2)) should give File2.

                var result = new List<string>();
                var commonOffset = 0;

                for (var chunkIndex = 0; chunkIndex < patch.Count; chunkIndex++)
                {
                    var chunk = patch[chunkIndex];
                    CopyCommon(chunk.File1.Offset, ref commonOffset, file, result);

                    for (var lineIndex = 0; lineIndex < chunk.File2.Chunk.Count; lineIndex++)
                        result.Add(chunk.File2.Chunk[lineIndex]);

                    commonOffset += chunk.File1.Length;
                }

                CopyCommon(file.Length, ref commonOffset, file, result);
                return result;
            }

            public static List<string> DiffMergeKeepall(string[] file1, string[] file2)
            {
                // Non-destructively merges two files.
                //
                // This is NOT a three-way merge - content will often be DUPLICATED by this process, eg
                // when starting from the same file some content was moved around on one of the copies.
                // 
                // To handle typical "Common ancestor" situations and avoid incorrect duplication of 
                // content, use diff3_merge instead.
                // 
                // This method's behaviour is similar to gnu diff's "if-then-else" (-D) format, but 
                // without the if/then/else lines!
                //

                var result = new List<string>();
                var file1CompletedToOffset = 0;
                var diffPatches = DiffPatch(file1, file2);

                for (var chunkIndex = 0; chunkIndex < diffPatches.Count; chunkIndex++)
                {
                    var chunk = diffPatches[chunkIndex];
                    if (chunk.File2.Length > 0)
                    {
                        //copy any not-yet-copied portion of File1 to the end of this patch entry
                        result.AddRange(file1.SliceJS(file1CompletedToOffset, chunk.File1.Offset + chunk.File1.Length));
                        file1CompletedToOffset = chunk.File1.Offset + chunk.File1.Length;

                        //copy the File2 portion of this patch entry
                        result.AddRange(chunk.File2.Chunk);
                    }
                }
                //copy any not-yet-copied portion of File1 to the end of the file
                result.AddRange(file1.SliceJS(file1CompletedToOffset, file1.Length));

                return result;
            }

            public static List<DiffSet> DiffIndices(string[] file1, string[] file2)
            {
                // We apply the LCS to give a simple representation of the
                // offsets and lengths of mismatched chunks in the input
                // files. This is used by diff3_merge_indices below.

                var result = new List<DiffSet>();
                var tail1 = file1.Length;
                var tail2 = file2.Length;

                for (var candidate = Diff.LongestCommonSubsequence(file1, file2);
                     candidate != null;
                     candidate = candidate.Chain)
                {
                    var mismatchLength1 = tail1 - candidate.File1Index - 1;
                    var mismatchLength2 = tail2 - candidate.File2Index - 1;
                    tail1 = candidate.File1Index;
                    tail2 = candidate.File2Index;

                    if (mismatchLength1 > 0 || mismatchLength2 > 0)
                    {
                        result.Add(new DiffSet
                        {
                            File1 = new ChunkReference
                            {
                                Offset = tail1 + 1,
                                Length = mismatchLength1
                            },
                            File2 = new ChunkReference
                            {
                                Offset = tail2 + 1,
                                Length = mismatchLength2
                            }
                        });
                    }
                }

                result.Reverse();
                return result;
            }

            private static void AddHunk(DiffSet h, Side side, List<Diff3Set> hunks)
            {
                hunks.Add(new Diff3Set
                {
                    Side = side,
                    File1Offset = h.File1.Offset,
                    File1Length = h.File1.Length,
                    File2Offset = h.File2.Offset,
                    File2Length = h.File2.Length
                });
            }

            private static void CopyCommon2(int targetOffset, ref int commonOffset, List<Patch3Set> result)
            {
                if (targetOffset > commonOffset)
                {
                    result.Add(new Patch3Set
                    {
                        Side = Side.Old,
                        Offset = commonOffset,
                        Length = targetOffset - commonOffset
                    });
                }
            }

            public static List<Patch3Set> Diff3MergeIndices(string[] a, string[] o, string[] b)
            {
                // Given three files, A, O, and B, where both A and B are
                // independently derived from O, returns a fairly complicated
                // internal representation of merge decisions it's taken. The
                // interested reader may wish to consult
                //
                // Sanjeev Khanna, Keshav Kunal, and Benjamin C. Pierce. "A
                // Formal Investigation of Diff3." In Arvind and Prasad,
                // editors, Foundations of Software Technology and Theoretical
                // Computer Science (FSTTCS), December 2007.
                //
                // (http://www.cis.upenn.edu/~bcpierce/papers/diff3-short.pdf)

                var m1 = Diff.DiffIndices(o, a);
                var m2 = Diff.DiffIndices(o, b);

                var hunks = new List<Diff3Set>();

                for (int i = 0; i < m1.Count; i++) { AddHunk(m1[i], Side.Left, hunks); }
                for (int i = 0; i < m2.Count; i++) { AddHunk(m2[i], Side.Right, hunks); }
                hunks.Sort();

                var result = new List<Patch3Set>();
                var commonOffset = 0;

                for (var hunkIndex = 0; hunkIndex < hunks.Count; hunkIndex++)
                {
                    var firstHunkIndex = hunkIndex;
                    var hunk = hunks[hunkIndex];
                    var regionLhs = hunk.File1Offset;
                    var regionRhs = regionLhs + hunk.File1Length;

                    while (hunkIndex < hunks.Count - 1)
                    {
                        var maybeOverlapping = hunks[hunkIndex + 1];
                        var maybeLhs = maybeOverlapping.File1Offset;
                        if (maybeLhs > regionRhs)
                            break;

                        regionRhs = Math.Max(regionRhs, maybeLhs + maybeOverlapping.File1Length);
                        hunkIndex++;
                    }

                    CopyCommon2(regionLhs, ref commonOffset, result);
                    if (firstHunkIndex == hunkIndex)
                    {
                        // The "overlap" was only one hunk long, meaning that
                        // there's no conflict here. Either a and o were the
                        // same, or b and o were the same.
                        if (hunk.File2Length > 0)
                        {
                            result.Add(new Patch3Set
                            {
                                Side = hunk.Side,
                                Offset = hunk.File2Offset,
                                Length = hunk.File2Length
                            });
                        }
                    }
                    else
                    {
                        // A proper conflict. Determine the extents of the
                        // regions involved from a, o and b. Effectively merge
                        // all the hunks on the left into one giant hunk, and
                        // do the same for the right; then, correct for skew
                        // in the regions of o that each Side changed, and
                        // report appropriate spans for the three sides.

                        var regions = new Dictionary<Side, ConflictRegion>
                        {
                            {
                                Side.Left,
                                new ConflictRegion
                                    {
                                        File1RegionStart = a.Length,
                                        File1RegionEnd = -1,
                                        File2RegionStart = o.Length,
                                        File2RegionEnd = -1
                                    }
                            },
                            {
                                Side.Right,
                                new ConflictRegion
                                    {
                                        File1RegionStart = b.Length,
                                        File1RegionEnd = -1,
                                        File2RegionStart = o.Length,
                                        File2RegionEnd = -1
                                    }
                            }
                        };

                        for (int i = firstHunkIndex; i <= hunkIndex; i++)
                        {
                            hunk = hunks[i];
                            var side = hunk.Side;
                            var r = regions[side];
                            var oLhs = hunk.File1Offset;
                            var oRhs = oLhs + hunk.File1Length;
                            var abLhs = hunk.File2Offset;
                            var abRhs = abLhs + hunk.File2Length;
                            r.File1RegionStart = Math.Min(abLhs, r.File1RegionStart);
                            r.File1RegionEnd = Math.Max(abRhs, r.File1RegionEnd);
                            r.File2RegionStart = Math.Min(oLhs, r.File2RegionStart);
                            r.File2RegionEnd = Math.Max(oRhs, r.File2RegionEnd);
                        }
                        var aLhs = regions[Side.Left].File1RegionStart + (regionLhs - regions[Side.Left].File2RegionStart);
                        var aRhs = regions[Side.Left].File1RegionEnd + (regionRhs - regions[Side.Left].File2RegionEnd);
                        var bLhs = regions[Side.Right].File1RegionStart + (regionLhs - regions[Side.Right].File2RegionStart);
                        var bRhs = regions[Side.Right].File1RegionEnd + (regionRhs - regions[Side.Right].File2RegionEnd);

                        result.Add(new Patch3Set
                        {
                            Side = Side.Conflict,
                            Offset = aLhs,
                            Length = aRhs - aLhs,
                            ConflictOldOffset = regionLhs,
                            ConflictOldLength = regionRhs - regionLhs,
                            ConflictRightOffset = bLhs,
                            ConflictRightLength = bRhs - bLhs
                        });
                    }

                    commonOffset = regionRhs;
                }

                CopyCommon2(o.Length, ref commonOffset, result);
                return result;
            }

            private static void FlushOk(List<string> okLines, List<IMergeResultBlock> result)
            {
                if (okLines.Count > 0)
                {
                    var okResult = new MergeOKResultBlock();
                    okResult.ContentLines = okLines.ToArray();
                    result.Add(okResult);
                }
                okLines.Clear();
            }

            private static bool IsTrueConflict(Patch3Set rec, string[] a, string[] b)
            {
                if (rec.Length != rec.ConflictRightLength)
                    return true;

                var aoff = rec.Offset;
                var boff = rec.ConflictRightOffset;

                for (var j = 0; j < rec.Length; j++)
                {
                    if (a[j + aoff] != b[j + boff])
                        return true;
                }
                return false;
            }

            public static List<IMergeResultBlock> Diff3Merge(string[] a, string[] o, string[] b, bool excludeFalseConflicts)
            {
                // Applies the output of Diff.diff3_merge_indices to actually
                // construct the merged file; the returned result alternates
                // between "ok" and "conflict" blocks.

                var result = new List<IMergeResultBlock>();
                var files = new Dictionary<Side, string[]>
                {
                    {Side.Left, a},
                    {Side.Old, o},
                    {Side.Right, b}
                };
                var indices = Diff.Diff3MergeIndices(a, o, b);

                var okLines = new List<string>();

                for (var i = 0; i < indices.Count; i++)
                {
                    var x = indices[i];
                    var side = x.Side;
                    if (side == Side.Conflict)
                    {
                        if (excludeFalseConflicts && !IsTrueConflict(x, a, b))
                        {
                            okLines.AddRange(files[0].SliceJS(x.Offset, x.Offset + x.Length));
                        }
                        else
                        {
                            FlushOk(okLines, result);
                            result.Add(new MergeConflictResultBlock
                            {
                                LeftLines = a.SliceJS(x.Offset, x.Offset + x.Length),
                                LeftIndex = x.Offset,
                                OldLines = o.SliceJS(x.ConflictOldOffset, x.ConflictOldOffset + x.ConflictOldLength),
                                OldIndex = x.ConflictOldOffset,
                                RightLines = b.SliceJS(x.ConflictRightOffset, x.ConflictRightOffset + x.ConflictRightLength),
                                RightIndex = x.Offset
                            });
                        }
                    }
                    else
                    {
                        okLines.AddRange(files[side].SliceJS(x.Offset, x.Offset + x.Length));
                    }
                }

                FlushOk(okLines, result);
                return result;
            }

            #endregion
        }
    }
}

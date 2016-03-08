using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// The code in this class except my algorithm for matching using color feature is from a C++ code written by
// Amiya Patanaik, Bibek Behera and Sukadeb Acharya - IIT Kharagpur - India. 
// http://aptnk.in/2008/08/automated-mosaicing-of-torn-paper-documents/ The file name is iConnect.cpp
// Because the original code fails to detect the matching edge for straight edges, I added the color on the edge as another metric
// for detecting matching edge
namespace TornRepair2
{
    
    public static class DNAUtil
    {
        // From Line 289-299
        public static List<Phi> replicateDNA(List<Phi> input)
        {
            List<Phi> linear = input.ToList();
            for (int i = 0; i < input.Count; ++i)
            {
                Phi temp;
                temp = input[i];
                temp.theta += 360; // might use this to get rid of negative numbers
                // also keeps the previous number if that is not negative
                linear.Add(temp);

            }
            return linear;
        }









        // Partial Match Algorithm
        // From Line 368-438
        public static Match partialMatch(List<Phi> DNAseq1, List<Phi> DNAseq2)
        {
            bool flag = true; // ToDo: Compare the control points in contours between two parts
            Match segment; // create an empty match segment
            segment.t11 = 0;
            segment.t12 = 0;
            segment.t21 = 0;
            segment.t22 = 0;

            List<Phi> seq1, seq2; // two empty List of edge maps
            int best = 0, max_match;
            int offset = 0, length = 0;
            // sticking
            // .While comparing a pair of DNA pattern, the smaller one is reversed while the longer one is copied and placed at its tail, 
            //such that now the angle accumulated over the string is 4π

            if (DNAseq1.Count > DNAseq2.Count) // if the contour in first part has more control points than the second part
            {
                seq1 = replicateDNA(DNAseq1);//replicate the larger DNA

                seq2 = DNAseq2.ToList();//reverse the smaller one
                seq2.Reverse();
            }
            else
            {
                flag = false;
                seq1 = replicateDNA(DNAseq2); // if the first one has less control point, attach all the control points of the second part
                seq2 = DNAseq1.ToList();//reverse the smaller one
                seq2.Reverse();
            }
            // sliding
            //The shorter sequence is slided through the longer one and for every shift ‘d’, the difference Δφabd = φbd – φa is stored
            for (int shift = 0; shift < seq1.Count - seq2.Count; shift += Constants.STEP)
            {
                List<int> diff = new List<int>();

                int start = 0, end = 0;
                for (int i = 0; i < seq2.Count; ++i)
                {
                    int difference = (int)(seq1[i + shift].theta - seq2[i].theta);
                    diff.Add(difference);
                }
                max_match = histogram(diff, seq2, ref start, ref end, Constants.DELTA_THETA);
                if (best < max_match)
                {
                    offset = shift;
                    best = max_match;
                    int t_start = start + shift;
                    int t_end = end + shift;
                    if (start + shift >= seq1.Count / 2)
                        t_start = start + shift - seq1.Count / 2;
                    if (end + shift >= seq1.Count / 2)
                        t_end = end + shift - seq1.Count / 2;
                    length = t_start - t_end;

                    if (flag)
                    {
                        segment.t21 = seq2.Count - end - 1;
                        segment.t22 = seq2.Count - start - 1;
                        segment.t11 = t_start;
                        segment.t12 = t_end;
                    }
                    else
                    {
                        segment.t11 = seq2.Count - end - 1;
                        segment.t12 = seq2.Count - start - 1;
                        segment.t21 = t_start;
                        segment.t22 = t_end;
                    }
                }
            }

            segment.x11 = (int)DNAseq1[segment.t11].x;
            segment.y11 = (int)DNAseq1[segment.t11].y;
            segment.x12 = (int)DNAseq1[segment.t12].x;
            segment.y12 = (int)DNAseq1[segment.t12].y;

            segment.x21 = (int)DNAseq2[segment.t21].x;
            segment.y21 = (int)DNAseq2[segment.t21].y;
            segment.x22 = (int)DNAseq2[segment.t22].x;
            segment.y22 = (int)DNAseq2[segment.t22].y;
            if (best == 0)
                segment.confidence = 0;
            else if (segment.t11 > segment.t12)
                segment.confidence = 0; // if the first segment run over the origin, reject the match

            else if (segment.t21 > segment.t22)
                segment.confidence = 0; // if the second segment run over the origin, reject the match
            else
                segment.confidence = Math.Sqrt((double)(length * length) / best);

            return segment;

        }

        // Partial Match Algorithm using color feature
        // I used the framework for the partial matching algorithm using turning angle
        // but using the differences of the color on the edge
        public static Match partialColorMatch(List<Phi> DNAseq1, List<Phi> DNAseq2)
        {
            bool flag = true; // ToDo: Compare the control points in contours between two parts
            Match segment; // create an empty match segment
            segment.t11 = 0;
            segment.t12 = 0;
            segment.t21 = 0;
            segment.t22 = 0;

            List<Phi> seq1, seq2; // two empty List of edge maps
            int best = 0, max_match;
            int offset = 0, length = 0;
            if (DNAseq1.Count > DNAseq2.Count) // if the contour in first part has more control points than the second part
            {
                seq1 = replicateDNA(DNAseq1);//replicate the larger DNA

                seq2 = DNAseq2.ToList();//reverse the smaller one
                seq2.Reverse();
            }
            else
            {
                flag = false;
                seq1 = replicateDNA(DNAseq2); // if the first one has less control point, attach all the control points of the second part
                seq2 = DNAseq1.ToList();//reverse the smaller one
                seq2.Reverse();
            }
            List<int> zc = new List<int>();
            List<int> starts = new List<int>();
            int iteration = 0;
            for (int shift = 0; shift < seq1.Count - seq2.Count; shift +=Constants.STEP)
            {
               
                List<int> diff = new List<int>();
                bool flag1 = false;
                int start = 0, end = 0;
                // TODO: change the differences into color difference (done)
                List<int> zeroCounts=new List<int>() ;
                int zeroCount = 0;
                List<int> starts2 = new List<int>();
                // TODO: need to add a tolerance level for some random non 0 differences
                int tolerCount = 0; // tolerance count for random non 0s.
                for (int i = 0; i < seq2.Count; ++i)
                {
                    int difference = Metrics.colorDifference(seq1[i + shift].color, seq2[i].color);
                    // if difference==0, flag
                    // if difference!=0, unflag
                    if (difference == 0)
                    {
                        // if it is in unflag state, mark the point as starting point
                        if (!flag1)
                        {
                            flag1 = true;
                            start = i;
                            //starts.Add(start);
                            starts2.Add(start);
                        }
                        // count the number of zero difference points in this section
                        zeroCount++;
                        tolerCount = 0;
                    }
                    else
                    {
                        if (tolerCount <= Constants.COLOR_TOLERANCE)
                        {
                            if (flag1)
                            {
                                zeroCount++;
                                tolerCount++;
                            }
                        }
                        else
                        {
                            if (flag1)
                            {
                                zeroCounts.Add(zeroCount); // add to a upper level storage
                                zeroCount = 0; // reset the counter
                                flag1 = false; // unflag
                                tolerCount = 0;
                            }
                        }
                    }


                    diff.Add(difference);
                }
                if (iteration == 33)
                {
                    Console.WriteLine("33");
                }
                if (zeroCounts.Count == 0)
                {
                    starts.Add(-1);
                }
                else
                {
                    starts.Add(starts2[zeroCounts.IndexOf(zeroCounts.Max())]);
                }
                if (zeroCounts.Count == 0)
                {
                    zc.Add(0);
                }
                else
                {
                    zc.Add(zeroCounts.Max());
                }
               
                // TTODO: implement a histogram algorithm for color match
                //max_match = colorHistogram(diff, seq2, ref start, ref end, Util.DELTA_THETA);
                max_match = 0;
                /*if (end < start)
                {
                    Console.WriteLine("22");
                }*/

                /* if (best < max_match)
                 {
                     offset = shift;
                     best = max_match;
                     int t_start = start + shift;
                     int t_end = end + shift;
                     if (start + shift >= seq1.Count / 2)
                         t_start = start + shift - seq1.Count / 2;
                     if (end + shift >= seq1.Count / 2)
                         t_end = end + shift - seq1.Count / 2;
                     length = t_start - t_end; // problematic


                     if (flag)
                     {
                         segment.t21 = seq2.Count - end - 1;
                         segment.t22 = seq2.Count - start - 1;
                         segment.t11 = t_start;
                         segment.t12 = t_end;
                     }
                     else
                     {
                         segment.t11 = seq2.Count - end - 1;
                         segment.t12 = seq2.Count - start - 1;
                         segment.t21 = t_start;
                         segment.t22 = t_end;
                     }
                 } */
                iteration++;
            }

            Console.WriteLine("Max:" + zc.Max());
            if (zc.Max() == 0)
            {
                goto a;
            }
            int t_shift = 0;
            int s_start = 0;
            for (int i = 0; i < zc.Count; i++)
            {
                if (zc[i] == zc.Max())
                {
                    t_shift = Constants.STEP * i;
                    s_start = starts[i];
                }
            }
            int startPos1 = t_shift + s_start;
            int endPos1 = startPos1 + zc.Max();
            int startPos2 = s_start;
            int endPos2 = startPos2 + zc.Max();
            length = zc.Max();
            // check if the algorithm get the correct position of the matching color
            Console.WriteLine("Flag:" + flag);
            Console.WriteLine("Shiftreq:" + startPos1);
            Console.WriteLine("Count:" + DNAseq1.Count);

            Console.WriteLine("P1_start_x" + seq1[startPos1].x);
            Console.WriteLine("P1_start_y" + seq1[startPos1].y);
            Console.WriteLine("P1_end_x" + seq1[endPos1].x);
            Console.WriteLine("P1_end_y" + seq1[endPos1].y);

            Console.WriteLine("P2_start_x" + seq2[startPos2].x);
            Console.WriteLine("P2_start_y" + seq2[startPos2].y);
            Console.WriteLine("P2_end_x" + seq2[endPos2].x);
            Console.WriteLine("P2_end_y" + seq2[endPos2].y);

            // correct for all the code above

            // regression analysis for the relationship between seq and DNA
            // flag=true for 3*3 frag5 and frag6
            if (flag)
            {

                for (int j = 0; j < DNAseq1.Count; j++)
                {
                    if ((seq1[startPos1].x == DNAseq1[j].x) && (seq1[startPos1].y == DNAseq1[j].y))
                    {
                        segment.t11 = j;
                        //segment.x11 = (int)DNAseq1[j].x;
                        // segment.y11 = (int)DNAseq1[j].y;
                        segment.t12 = j + zc.Max();
                        if (segment.t12 >= DNAseq1.Count)
                        {
                            segment.t12 -= DNAseq1.Count;
                        }
                        //segment.x12 = (int)DNAseq1[j + zc.Max()].x;
                        //segment.y12 = (int)DNAseq1[j + zc.Max()].y;
                    }
                }


                for (int j = 0; j < DNAseq2.Count; j++)
                {
                    if ((seq2[startPos2].x == DNAseq2[j].x) && (seq2[startPos2].y == DNAseq2[j].y))
                    {
                        segment.t21 = j;
                        //segment.x21 = (int)DNAseq2[j].x;
                        //segment.y21 = (int)DNAseq2[j].y;
                        segment.t22 = j - zc.Max();
                        if (segment.t22 < 0)
                        {
                            segment.t22 += DNAseq2.Count;
                        }
                        //segment.x22 = (int)DNAseq2[j - zc.Max()].x;
                        //segment.y22 = (int)DNAseq2[j - zc.Max()].y;
                    }
                }

            }
            else
            {
                for (int j = 0; j < DNAseq2.Count; j++)
                {
                    if ((seq1[startPos1].x == DNAseq2[j].x) && (seq1[startPos1].y == DNAseq2[j].y))
                    {
                        segment.t21 = j;
                        //segment.x11 = (int)DNAseq2[j].x;

                        //segment.y11 = (int)DNAseq2[j].y;
                        segment.t22 = j + zc.Max();
                        if (segment.t22 >= DNAseq2.Count)
                        {
                            segment.t22 -= DNAseq2.Count;
                        }
                        // segment.x12 = (int)DNAseq2[j - zc.Max()].x;
                        //segment.y12 = (int)DNAseq2[j - zc.Max()].y;
                    }
                }

                for (int j = 0; j < DNAseq1.Count; j++)
                {
                    if ((seq2[startPos2].x == DNAseq1[j].x) && (seq2[startPos2].y == DNAseq1[j].y))
                    {
                        segment.t11 = j;
                        // segment.x21 = (int)DNAseq1[j].x;
                        // segment.y21 = (int)DNAseq1[j].y;
                        segment.t12 = j - zc.Max();
                        if (segment.t12 < 0)
                        {
                            segment.t12 += DNAseq1.Count;
                        }
                        //segment.x22 = (int)DNAseq1[j + zc.Max()].x;
                        //segment.y22 = (int)DNAseq1[j + zc.Max()].y;
                    }
                }

            }



            /*int t_start = t_shift;
            int t_end = t_shift;
            if (t_shift >= seq1.Count / 2)
                t_start = t_shift - seq1.Count / 2;
            if (t_shift >= seq1.Count / 2)
                t_end = t_shift - seq1.Count / 2;
            length = zc.Max(); // problematic


            /*if (flag)
            {
                segment.t21 = seq2.Count - -1;
                segment.t22 = seq2.Count - t_start - 1;
                segment.t11 = t_start;
                segment.t12 = t_end;
            }
            else
            {
                segment.t11 = seq2.Count - t_end - 1;
                segment.t12 = seq2.Count - t_start - 1;
                segment.t21 = t_start;
                segment.t22 = t_end;
            }*/

            // fine code below


            a: segment.x11 = (int)DNAseq1[segment.t11].x;
            segment.y11 = (int)DNAseq1[segment.t11].y;
            segment.x12 = (int)DNAseq1[segment.t12].x;
            segment.y12 = (int)DNAseq1[segment.t12].y;

            segment.x21 = (int)DNAseq2[segment.t21].x;
            segment.y21 = (int)DNAseq2[segment.t21].y;
            segment.x22 = (int)DNAseq2[segment.t22].x;
            segment.y22 = (int)DNAseq2[segment.t22].y;

            // correct at this point
            /*if (best == 0)
                segment.confidence = 0;
            else
                segment.confidence = Math.Sqrt((double)(length * length) / best); */
            segment.confidence = length;
            Console.WriteLine(segment.ToString());
            return segment;

        }

        // TODO: try from 0 to 4, use edge map
        // From Line 313-366
        // this method is used to detect the portion on the matching edge that has similar turning angle difference
        // the portion that has the largest length is the matching edge
        private static int histogram(List<int> diff, List<Phi> seq, ref int t_start, ref int t_end, int delta_theta = 5)
        {
            int max_theta, min_theta;
            extreme(diff, out min_theta, out max_theta); // get the min  angle difference and max angle difference

            int max_points = 0, range = 0, change = 0;
            int startt = 0, endd = 0;
            for (int i = min_theta; i <= max_theta - delta_theta; i += delta_theta)
            {
                int points = 0, _change = 0;
                List<int> thetas = new List<int>();
                bool flag = false;
                for (int j = 0; j < diff.Count; ++j)
                {
                   // Since the DNA signature increases monotonically, at the point where the pattern matches, the difference is nearly a constant. 
                    if (diff[j] >= i && diff[j] < i + delta_theta) // if the difference lies in the 
                    {
                        if (!flag)
                        {
                            startt = j;
                            flag = true;
                        }
                        endd = j;
                        points++;//Points that lie in sampling zone
                        thetas.Add((int)seq[j].theta);
                    }
                }

                //apply conditions
                _change = changes(thetas);
                // finding the best valid match, only edges with at least 3 turning angles can be counted as a match
                if (max_points < points && _change > 3)
                {
                    max_points = points;
                    change = _change;
                    range = i;
                    t_start = startt;
                    t_end = endd;
                }
            }
            // this actually means if a match is found or not
            // this corrects the start point and end point
            if (t_end - t_start > max_points * Constants.MULT)
            {
                int max_count = 0;
                int offset = 0;
                // max_points ( turning points) * multiplier ( min turning angle accepted)= the length of "organized" points
                for (int shift = 0; shift < t_end - t_start - max_points * Constants.MULT; shift++)
                {
                    int count = 0;
                    for (int i = 0; i < max_points * Constants.MULT; ++i)
                    {
                        if (diff[t_start + i + shift] >= range && diff[t_start + i + shift] < range + delta_theta)
                            count++;
                    }
                    // find the best start point
                    if (max_count < count)
                    {
                        max_count = count;
                        offset = shift;
                    }
                }
                t_start += offset;
                t_end = t_start + max_count * Constants.MULT;
            }
            return change;

        }
        // calculate the turning angle differences in a specific sampling area
        // the threshold for a change is 3
        // Ex Point 0=0, Point 1=1, Point 2=4, Point 3=5, Point 4=8
        // i=0 p1-p0=1 -> p2-p0 =4 , initial = x[2]=4
        // i=2 p3-p2=1 -> p4-p2= 4, initial=x[4]=8
        // count =2
        // From Line 264-278
        
        private static int changes(List<int> X)
        {
            if (X.Count == 0)
                return 0;

            int initial = X[0];
            int count = 0;
            for (int i = 1; i < X.Count; ++i)
            {
                if (Math.Abs(X[i] - initial) > Constants.MIN_TURN)
                {
                    count++;
                    initial = X[i];
                }
            }
            return count;
        }

        // From Line 301-311
        // calculate the min and max element in a list 
        private static void extreme(List<int> input, out int min, out int max)
        {
            max = input[0];
            min = input[0];
            for (int i = 1; i < input.Count; ++i)
            {
                if (max < input[i])
                    max = input[i];
                if (min > input[i])
                    min = input[i];
            }
        }
    }
}

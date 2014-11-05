using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Sentiment
{

    /*
    product/productId: B000P41A28
    review/userId: AFAWC4ZJ29YV1
    review/profileName: AG
    review/helpfulness: 1/3
    review/score: 1.0
    review/time: 1334102400
    review/summary: THIS IS NOT ORGANIC!!! TOXIC hexane still in it!
    review/text: RECONSIDER THIS FORMULA!!<br /><br />Martek advertises this formula on their website!<br /><br />"Although Martek told the board that they would discontinue the use of the controversial neurotoxic solvent n-hexane for DHA/ARA processing, they did not disclose what other synthetic solvents would be substituted. Federal organic standards prohibit the use of all synthetic/petrochemical solvents".<br /><br />Martek Biosciences was able to dodge the ban on hexane-extraction by claiming USDA does not consider omega-3 and omega-6 fats to be "agricultural ingredients." Therefore, they argue, the ban against hexane extraction does not apply. The USDA helped them out by classifying those oils as "necessary vitamins and minerals," which are exempt from the hexane ban. But hexane-extraction is just the tip of the iceberg. Other questionable manufacturing practices and misleading statements by Martek included:<br /><br />Undisclosed synthetic ingredients, prohibited for use in organics (including the sugar alcohol mannitol, modified starch, glucose syrup solids, and "other" undisclosed ingredients)<br />Microencapsulation of the powder and nanotechnology, which are prohibited under organic laws<br />Use of volatile synthetic solvents, besides hexane (such as isopropyl alcohol)<br />Recombinant DNA techniques and other forms of genetic modification of organisms; mutagenesis; use of GMO corn as a fermentation medium<br />Heavily processed ingredients that are far from "natural"<br /><br />quote from: Why is this Organic Food Stuffed With Toxic Solvents? by Dr. Mercola - GOOGLE GMOs found in Martek.<br /><br />This is the latest I have found on DHA in organic and non organic baby food/ formula:<br />AT LEAST READ THIS ONE*** GOOGLE- False Claims That DHA in Organic and Non-Organic Infant Formula Is Safe. AND OrganicconsumersDOTorg<br /><br />Martek's patents for Life'sDHA states: "includes mutant organisms" and "recombinant organisms", (a.k.a. GMOs!) The patents explain that the oil is "extracted readily with an effective amount of solvent ... a preferred solvent is hexane."<br /><br />The patent for Life'sARA states: "genetically-engineering microorganisms to produce increased amounts of arachidonic acid" and "extraction with solvents such as ... hexane." Martek has many other patents for DHA and ARA. All of them include GMOs. GMOs and volatile synthetic solvents like hexane aren't allowed in USDA Organic products and ingredients.<br /><br />Tragically, Martek's Life'sDHA is already in hundreds of products, many of them certified USDA Organic. Please demand that the National Organic Standards Board reject Martek's petition, and that the USDA National Organic Program inform the company that the illegal 2006 approval is rescinded and that their GMO, hexane-extracted Life'sDHA and Life'sARA are no longer allowed in organic products.<br /><br />BUT I went to the lifesdha website and THEY DO NOT DISCLOSE HOW THEY MAKE THEIR LifesDHA!!! I have contacted the company to see what they say.<br /><br />Also these are the corporate practices of Martek which are damaging to the environment as well written just last Dec 2011 at NaturalnewsDOTcom<br /><br />The best bet is to just avoid the lifeDHA at this time in my opinion b/c corporate america cares more about the almighty $ than your health.
 */

    public class TestDataParser
    {
        private const string ReviewPatternStart = "review/";

        private const string ProductIdPattern = @"product/productId:\s(.+)";
        private const string UserIdPattern = ReviewPatternStart + @"userId:\s(.+)";
        private const string ProfileNamePattern = ReviewPatternStart + @"profileName:\s(.+)";
        private const string HelpfulnessPattern = ReviewPatternStart + @"helpfulness:\s(\d)/(\d)";
        private const string ScorePattern = ReviewPatternStart + @"score:\s(.+)";
        private const string TimePattern = ReviewPatternStart+@"time:\s(.+)";
        private const string SummaryPattern = ReviewPatternStart+@"summary:\s(.+)";
        private const string TextPattern = ReviewPatternStart+@"text:\s(.+)";

        private readonly string _filePath;
        private List<Review> _reviews;

        public List<Review> Reviews
        {
            get { return _reviews; }
        }

        public TestDataParser(string filePath)
        {
            this._filePath = filePath;
            _reviews = new List<Review>();
        }

        public void Parse()
        {
            StreamReader reader = new StreamReader(_filePath);
            Regex productRegex = new Regex(ProductIdPattern, RegexOptions.IgnoreCase);
            Regex userIdRegex = new Regex(UserIdPattern, RegexOptions.IgnoreCase);
            Regex profileNameRegex = new Regex(ProfileNamePattern, RegexOptions.IgnoreCase);
            Regex helpfulnessRegex = new Regex(HelpfulnessPattern, RegexOptions.IgnoreCase);
            Regex scoreRegex = new Regex(ScorePattern, RegexOptions.IgnoreCase);
            Regex timeRegex = new Regex(TimePattern, RegexOptions.IgnoreCase);
            Regex summaryRegex = new Regex(SummaryPattern, RegexOptions.IgnoreCase);
            Regex textRegex = new Regex(TextPattern, RegexOptions.IgnoreCase);

            string productId = null;
            string userId = null;
            string profileName = null;
            double helpfulness = -1;
            double score = -1;
            long time = -1;
            string summary = null;
            string text = null;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (line == null)
                    continue;

                if (productRegex.IsMatch(line))
                {
                    productId = productRegex.Match(line).Groups[1].Value;
                }
                else if (userIdRegex.IsMatch(line))
                {
                    userId = userIdRegex.Match(line).Groups[1].Value;
                }
                else if (profileNameRegex.IsMatch(line))
                {
                    profileName = profileNameRegex.Match(line).Groups[1].Value;
                }
                else if (helpfulnessRegex.IsMatch(line))
                {
                    var groups = helpfulnessRegex.Match(line).Groups;
                    helpfulness = double.Parse(groups[1].Value) / double.Parse(groups[2].Value);
                }
                else if (scoreRegex.IsMatch(line))
                {
                    string s = scoreRegex.Match(line).Groups[1].Value.Replace('.', ',');
                    score = Convert.ToDouble(s);
                }
                else if (timeRegex.IsMatch(line))
                {
                    time = long.Parse(timeRegex.Match(line).Groups[1].Value);
                }
                else if (summaryRegex.IsMatch(line))
                {
                    summary = summaryRegex.Match(line).Groups[1].Value;
                }
                else if (textRegex.IsMatch(line))
                {
                    text = textRegex.Match(line).Groups[1].Value;
                    _reviews.Add(new Review(productId, userId, profileName, helpfulness, score, time, summary, text));
                    productId = null;
                    userId = null;
                    profileName = null;
                    helpfulness = -1;
                    score = -1;
                    time = -1;
                    summary = null;
                    text = null;
                }
            }
        }

    }
}

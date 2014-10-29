using System;
using System.Collections.Generic;

namespace EigenValueDecomposition
{
    class Review
    {
        private Label rating;
        private string reviewText;

        public Review(string rating, string reviewText)
        {
            Enum.TryParse(rating, true, out this.rating);
            this.reviewText = reviewText;
        }

        public Label Rating
        {
            get { return rating; }
        }

        public string ReviewText
        {
            get { return reviewText; }
        }
    }
}
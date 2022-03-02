using LPA.UI;

namespace LPA.Application.Progress
{
    internal class ProgressWithValue
        : Progress,
          IProgressWithValue
    {
        private float progressValue = 0.0f;

        public ProgressWithValue(ILpaWindowManager lpaWindowManager)
            : base(lpaWindowManager)
        {
        }

        public float Value
        {
            get
            {
                return this.progressValue;
            }
            set
            {
                if (this.progressValue != value)
                {
                    this.progressValue = value;
                    InvalidateData();
                }
            }
        }
    }
}

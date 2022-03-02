namespace LPA.UI.ResponseObjects.Progress
{
    public struct ProgressState
    {
        public string Label { get; set; }

        public string CancelText { get; set; }

        public bool CanCancel { get; set; }

        public float? Value { get; set; }
    }
}
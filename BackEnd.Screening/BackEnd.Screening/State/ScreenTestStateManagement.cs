namespace BackEnd.Screening.State
{
    public static class ScreenTestStateManagement
    {
        public static bool IsLoggedIn { get; set; } = false;
        public static bool RedirectToLogin { get; set; } = false;
        public static bool IsLoginSuccessfully { get; set; } = false;
        public static bool FilteredSearch { get; set; } = false;
        public static bool IsFirstPageGathered { get; set; } = false;
        public static bool IsSecondPageWorked { get; set; } = false;
        public static bool IsSecondPageGathered { get; set; } = false;
        public static bool IsPassedSecondPage { get; set; } = false;
        public static bool IsCarDetailOpen { get; set; } = false;
        public static bool IsCarDetailGathered { get; set; } = false;
        public static bool IsHomeDeliveryGathered { get; set; } = false;
        public static bool IsModelSProcessCompleted { get; set; } = false;
        public static bool IsModelXProcessCompleted { get; set; } = false;
        public static bool IsReadyToFilterModelX { get; set; } = false;
        public static bool IsFilteredForModelX { get; set; } = false;
        public static bool InModelXSearchResult { get; set; } = false;
        public static int SearcResultLoadCount { get; set; } = 0;

        public static void ResetGatherinStates()
        {
            IsFilteredForModelX = true;
            IsFirstPageGathered = false;
            IsPassedSecondPage = false;
            IsSecondPageGathered = false;
            IsSecondPageWorked = false;
            IsCarDetailOpen = false;
            IsCarDetailGathered = false;
        }
    }
}

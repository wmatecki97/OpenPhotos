namespace OpenPhotos.Core.ImaggaModels
{
    internal class ImaggaTagResponseModel
    {
        public ResultModel Result { get; set; }
    }

    internal class ResultModel
    {
        public TagModelWithProbability[] Tags { get; set; }
    }
    internal class TagModelWithProbability
    {
        public double Confidence { get; set; }
        public Dictionary<string, string> Tag{ get; set; }
        public string TagValue => Tag.Values.First();
    }
}

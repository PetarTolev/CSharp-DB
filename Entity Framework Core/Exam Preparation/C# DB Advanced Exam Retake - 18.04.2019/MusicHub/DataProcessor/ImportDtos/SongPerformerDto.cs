namespace MusicHub.DataProcessor.ImportDtos
{
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class SongPerformerDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
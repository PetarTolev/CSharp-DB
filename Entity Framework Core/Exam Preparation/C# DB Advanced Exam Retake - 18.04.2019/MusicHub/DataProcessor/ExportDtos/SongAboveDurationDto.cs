using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ExportDtos
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [XmlType("Song")]
    public class SongAboveDurationDto
    {
        public string SongName { get; set; }

        public string Writer { get; set; }

        public string Performer { get; set; }

        public string AlbumProducer { get; set; }

        public string Duration { get; set; }
    }
}
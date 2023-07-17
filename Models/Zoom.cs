using AbsolCase.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsolCase.Models
{
    public class Zoom
    {
        public string Host { get; set; }
        public string Join { get; set; }
        public string Code { get; set; }
        public string shareurl { get; set; }
        public string meetingId { get; set; }
        public string passcode { get; set; }
        public string title { get; set; }
        

    }



    public class Meetingresult
    {
        [NotMapped]
        public string from { get; set; }
        [NotMapped]
        public string to { get; set; }
        [NotMapped]
        public string page_count { get; set; }
        [NotMapped]
        public string page_size { get; set; }
        [NotMapped]
        public string total_records { get; set; }
        [NotMapped]
        public string next_page_token { get; set; }
        public List<meetings> meetings { get; set; }


    }
    public class meetings
    {
        public string uuid { get; set; }
        public string id { get; set; }
        public string account_id { get; set; }
        public string host_id { get; set; }
        public string topic { get; set; }
        public string type { get; set; }
        public string start_time { get; set; }
        public string timezone { get; set; }
        public string duration { get; set; }
        public string total_size { get; set; }
        public string recording_count { get; set; }
        public string share_url { get; set; }
        public List<recording_files> recording_files { get; set; }
    }
    public class recording_files
    {
        public string id { get; set; }
        public string meeting_id { get; set; }
        public string recording_start { get; set; }
        public string recording_end { get; set; }
        public string file_type { get; set; }
        public string file_extension { get; set; }
        public string file_size { get; set; }
        public string play_url { get; set; }
        public string download_url { get; set; }
        public string status { get; set; }
        public string recording_type { get; set; }
        
    }
    public class meetingDetails
    {
        public string id { get; set; }
        public string uuid { get; set; }
        public string account_id { get; set; }
        public string host_id { get; set; }
        public string topic { get; set; }
        public string type { get; set; }
        public string start_time { get; set; }
        public string timezone { get; set; }
        public string host_email { get; set; }
        public string duration { get; set; }
        public string total_size { get; set; }
        public string recording_count { get; set; }
        public string share_url { get; set; }
        public string password { get; set; }
        public string recording_play_passcode { get; set; }
        
    }


}

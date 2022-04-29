using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SML___Pokayoke_System
{
    class sound
    {
        //How to use [ sound sound = new sound(); / sound.alarm_sounnd(true); ]
        internal void alarm_sounnd(bool temp_sound)
        {
            bool flag_sound;
            flag_sound = temp_sound;
            //set folder of sound
            const string folder_path = @"C:\SSS\sound";
            string sound_path;

            //true is correct sound / false is wrong sound.
            if (flag_sound)
            {
                sound_path = $@"{folder_path}\correct.wav";
            }
            else
            {
                sound_path = $@"{folder_path}\wrong.wav";
            }
            string sound_file = sound_path;

            byte[] bt = File.ReadAllBytes(sound_file);
            var sound = new System.Media.SoundPlayer(sound_file);
            sound.Play();
        }
    }
}

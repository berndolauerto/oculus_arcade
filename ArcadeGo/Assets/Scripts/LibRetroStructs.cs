using System;
using System.Runtime.InteropServices;

namespace LibRetroWrapper
{
    public class RetroStructs
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RetroGameGeometry
        {
            public uint base_width;
            public uint base_height;
            public uint max_width;
            public uint max_height;
            public float aspect_ratio;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct RetroGameInfo
        {
            public char* path;
            public void* data;
            public uint size;
            public char* meta;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct RetroMessage
        {
            public char* msg;
            public uint frames;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct RetroLog
        {
            public IntPtr log;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct RetroInputDescriptor
        {
            public uint port;
            public uint device;
            public uint index;
            public uint id;

            public char* description;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RetroSystemAVInfo
        {
            public RetroGameGeometry geometry;
            public RetroSystemTiming timing;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct RetroSystemInfo
        {

            public char* library_name;
            public char* library_version;
            public char* valid_extensions;

            [MarshalAs(UnmanagedType.U1)]
            public bool need_fullpath;

            [MarshalAs(UnmanagedType.U1)]
            public bool block_extract;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RetroSystemTiming
        {
            public double fps;
            public double sample_rate;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct RetroVariable
        {
            public char* key;
            public char* value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct RetroRumbleInterface
        {
            public IntPtr callback;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct RetroDiskControlCallback
        {
            public IntPtr set_eject_state;
            public IntPtr get_eject_State;

            public IntPtr get_image_index;
            public IntPtr set_image_index;
            public IntPtr get_num_images;

            public IntPtr replace_image_index;
            public IntPtr add_image_index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct RetroVfsInterfaceInfo
        {
            public uint required_interface_version;
            public IntPtr iface;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct RetroVfsInterface
        {
            public IntPtr get_path;
            public IntPtr open;
            public IntPtr close;
            public IntPtr size;
            public IntPtr tell;
            public IntPtr seek;
            public IntPtr read;
            public IntPtr write;
            public IntPtr flush;
            public IntPtr remove;
            public IntPtr rename;
            public IntPtr truncate;
        };
    }
}

using System;
using System.Runtime.InteropServices;

namespace LibRetroWrapper
{
    public class DelegateDefinition
    {
        //typedef void (*retro_video_refresh_t)(const void *data, unsigned width, unsigned height, size_t pitch);
        public unsafe delegate void RetroVideoRefreshDelegate(void* data, uint width, uint height, uint pitch);

        //typedef void (*retro_audio_sample_t)(int16_t left, int16_t right);
        public unsafe delegate void RetroAudioSampleDelegate(short left, short right);

        //typedef size_t (*retro_audio_sample_batch_t)(const int16_t *data, size_t frames);
        public unsafe delegate void RetroAudioSampleBatchDelegate(short* data, uint frames);

        //typedef void (*retro_input_poll_t)(void);
        public delegate void RetroInputPollDelegate();

        //typedef int16_t (*retro_input_state_t)(unsigned port, unsigned device, unsigned index, unsigned id);
        public delegate Int16 RetroInputStateDelegate(uint port, uint device, uint index, uint id);

        //typedef bool (*retro_environment_t)(unsigned cmd, void *data);
        public unsafe delegate bool RetroEnvironmentDelegate(uint cmd, void* data);

        //typedef void (*retro_log_printf_t) (enum retro_log_level level, const char* fmt, ...);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate void RetroLogDelegate(int log_level, string format, IntPtr args);

        //typedef bool (RETRO_CALLCONV* retro_set_rumble_state_t) (unsigned port,
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate bool RetroRumbleDelegate(uint port, int effect, ushort strength);

        //typedef bool (RETRO_CALLCONV* retro_set_eject_state_t) (bool ejected);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate bool RetroSetEjectState(bool ejected);

        //typedef bool (RETRO_CALLCONV* retro_get_eject_state_t) (void);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate bool RetroGetEjectState();

        //typedef unsigned(RETRO_CALLCONV* retro_get_image_index_t)(void);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate uint RetroGetImageIndex();

        //typedef bool (RETRO_CALLCONV* retro_set_image_index_t) (unsigned index);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate bool RetroSetImageIndex(uint index);

        //typedef unsigned(RETRO_CALLCONV* retro_get_num_images_t)(void);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate uint RetroGetNumImages();

        //typedef bool (RETRO_CALLCONV* retro_replace_image_index_t) (unsigned index,const struct retro_game_info *info);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate bool RetroReplaceImageIndex(uint index, ref RetroStructs.RetroGameInfo info);

        //typedef bool (RETRO_CALLCONV* retro_add_image_index_t) (void);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate bool RetroAddImageIndex();

        //typedef const char*(RETRO_CALLCONV * retro_vfs_get_path_t)(struct retro_vfs_file_handle *stream);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate char* RetroVfsGetPath(IntPtr stream);

        //typedef struct retro_vfs_file_handle *(RETRO_CALLCONV* retro_vfs_open_t) (const char* path, unsigned mode, unsigned hints);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate IntPtr RetroVfsOpen(string path, uint mode, uint hints);

        //typedef int (RETRO_CALLCONV* retro_vfs_close_t) (struct retro_vfs_file_handle *stream);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate int RetroVfsClose(IntPtr stream);

        //typedef int64_t(RETRO_CALLCONV* retro_vfs_size_t)(struct retro_vfs_file_handle *stream);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate ulong RetroVfsSize(IntPtr stream);

        //typedef int64_t(RETRO_CALLCONV* retro_vfs_truncate_t)(struct retro_vfs_file_handle *stream, int64_t length);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate ulong RetroVfsTruncate(IntPtr stream, ulong length);

        //typedef int64_t(RETRO_CALLCONV* retro_vfs_tell_t)(struct retro_vfs_file_handle *stream);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate ulong RetroVfsTell(IntPtr stream);

        //typedef int64_t(RETRO_CALLCONV* retro_vfs_seek_t)(struct retro_vfs_file_handle *stream, int64_t offset, int seek_position);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate ulong RetroVfsSeek(IntPtr stream, ulong offset, int seek_position);

        //typedef int64_t(RETRO_CALLCONV* retro_vfs_read_t)(struct retro_vfs_file_handle *stream, void* s, uint64_t len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate ulong RetroVfsRead(IntPtr stream, IntPtr s, ulong len);

        //typedef int64_t(RETRO_CALLCONV* retro_vfs_write_t)(struct retro_vfs_file_handle *stream, const void* s, uint64_t len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate ulong RetroVfsWrite(IntPtr stream, IntPtr s, ulong len);

        //typedef int (RETRO_CALLCONV* retro_vfs_flush_t) (struct retro_vfs_file_handle *stream);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate int RetroVfsFlush(IntPtr stream);

        //typedef int (RETRO_CALLCONV* retro_vfs_remove_t) (const char* path);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate int RetroVfsRemove(string path);

        //typedef int (RETRO_CALLCONV* retro_vfs_rename_t) (const char* old_path, const char* new_path);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate int RetroVfsRename(string old_path, string new_path);
    }
}

namespace LibRetroWrapper
{
    public class RetroEnums
    {
        public enum RetroFileOpenFlags : uint
        {
            RETRO_VFS_FILE_ACCESS_READ = (1 << 0),
            RETRO_VFS_FILE_ACCESS_WRITE = (1 << 1),
            RETRO_VFS_FILE_ACCESS_READ_WRITE = (RETRO_VFS_FILE_ACCESS_READ | RETRO_VFS_FILE_ACCESS_WRITE),
            RETRO_VFS_FILE_ACCESS_UPDATE_EXISTING = (1 << 2)
        }

        public enum RetroFileHints : uint
        {
            RETRO_VFS_FILE_ACCESS_HINT_NONE = 0,
            RETRO_VFS_FILE_ACCESS_HINT_FREQUENT_ACCESS = (1 << 0)
        }

        public enum RetroSeekPositions : uint
        {
            RETRO_VFS_SEEK_POSITION_START = 0,
            RETRO_VFS_SEEK_POSITION_CURRENT = 1,
            RETRO_VFS_SEEK_POSITION_END = 2
        }

        public enum RetroDevices : uint
        {
            RETRO_DEVICE_NONE = 0,
            RETRO_DEVICE_JOYPAD = 1,
            RETRO_DEVICE_MOUSE = 2,
            RETRO_DEVICE_KEYBOARD = 3,
            RETRO_DEVICE_LIGHTGUN = 4,
            RETRO_DEVICE_ANALOG = 5,
            RETRO_DEVICE_POINTER = 6
        }

        public enum RetroButtons : uint
        {
            RETRO_DEVICE_ID_JOYPAD_B = 0,
            RETRO_DEVICE_ID_JOYPAD_Y = 1,
            RETRO_DEVICE_ID_JOYPAD_SELECT = 2,
            RETRO_DEVICE_ID_JOYPAD_START = 3,
            RETRO_DEVICE_ID_JOYPAD_UP = 4,
            RETRO_DEVICE_ID_JOYPAD_DOWN = 5,
            RETRO_DEVICE_ID_JOYPAD_LEFT = 6,
            RETRO_DEVICE_ID_JOYPAD_RIGHT = 7,
            RETRO_DEVICE_ID_JOYPAD_A = 8,
            RETRO_DEVICE_ID_JOYPAD_X = 9,
            RETRO_DEVICE_ID_JOYPAD_L = 10,
            RETRO_DEVICE_ID_JOYPAD_R = 11,
            RETRO_DEVICE_ID_JOYPAD_L2 = 12,
            RETRO_DEVICE_ID_JOYPAD_R2 = 13,
            RETRO_DEVICE_ID_JOYPAD_L3 = 14,
            RETRO_DEVICE_ID_JOYPAD_R3 = 15
        }

        public enum RetroAnalog : uint
        {
            RETRO_DEVICE_INDEX_ANALOG_LEFT = 0,
            RETRO_DEVICE_INDEX_ANALOG_RIGHT = 1,
            RETRO_DEVICE_INDEX_ANALOG_BUTTON = 2,
            RETRO_DEVICE_ID_ANALOG_X = 0,
            RETRO_DEVICE_ID_ANALOG_Y = 1
        }

        public enum RetroMouse : uint
        {
            RETRO_DEVICE_ID_MOUSE_X = 0,
            RETRO_DEVICE_ID_MOUSE_Y = 1,
            RETRO_DEVICE_ID_MOUSE_LEFT = 2,
            RETRO_DEVICE_ID_MOUSE_RIGHT = 3,
            RETRO_DEVICE_ID_MOUSE_WHEELUP = 4,
            RETRO_DEVICE_ID_MOUSE_WHEELDOWN = 5,
            RETRO_DEVICE_ID_MOUSE_MIDDLE = 6,
            RETRO_DEVICE_ID_MOUSE_HORIZ_WHEELUP = 7,
            RETRO_DEVICE_ID_MOUSE_HORIZ_WHEELDOWN = 8,
            RETRO_DEVICE_ID_MOUSE_BUTTON_4 = 9,
            RETRO_DEVICE_ID_MOUSE_BUTTON_5 = 10
        }

        public enum RetroLightgun : uint
        {
            RETRO_DEVICE_ID_LIGHTGUN_SCREEN_X = 13,
            RETRO_DEVICE_ID_LIGHTGUN_SCREEN_Y = 14,
            RETRO_DEVICE_ID_LIGHTGUN_IS_OFFSCREEN = 15,
            RETRO_DEVICE_ID_LIGHTGUN_TRIGGER = 2,
            RETRO_DEVICE_ID_LIGHTGUN_RELOAD = 16,
            RETRO_DEVICE_ID_LIGHTGUN_AUX_A = 3,
            RETRO_DEVICE_ID_LIGHTGUN_AUX_B = 4,
            RETRO_DEVICE_ID_LIGHTGUN_START = 6,
            RETRO_DEVICE_ID_LIGHTGUN_SELECT = 7,
            RETRO_DEVICE_ID_LIGHTGUN_AUX_C = 8,
            RETRO_DEVICE_ID_LIGHTGUN_DPAD_UP = 9,
            RETRO_DEVICE_ID_LIGHTGUN_DPAD_DOWN = 10,
            RETRO_DEVICE_ID_LIGHTGUN_DPAD_LEFT = 11,
            RETRO_DEVICE_ID_LIGHTGUN_DPAD_RIGHT = 12,
            RETRO_DEVICE_ID_LIGHTGUN_X = 0,
            RETRO_DEVICE_ID_LIGHTGUN_Y = 1,
            RETRO_DEVICE_ID_LIGHTGUN_CURSOR = 3,
            RETRO_DEVICE_ID_LIGHTGUN_TURBO = 4,
            RETRO_DEVICE_ID_LIGHTGUN_PAUSE = 5
        }

        public enum RetroPointer
        {
            RETRO_DEVICE_ID_POINTER_X = 0,
            RETRO_DEVICE_ID_POINTER_Y = 1,
            RETRO_DEVICE_ID_POINTER_PRESSED = 2
        }

        public enum RetroRegion
        {
            RETRO_REGION_NTSC = 0,
            RETRO_REGION_PAL = 1
        }

        public enum ConfigurationConstants : uint
        {
            RETRO_ENVIRONMENT_SET_ROTATION = 1,
            RETRO_ENVIRONMENT_GET_OVERSCAN = 2,
            RETRO_ENVIRONMENT_GET_CAN_DUPE = 3,
            RETRO_ENVIRONMENT_SET_MESSAGE = 6,
            RETRO_ENVIRONMENT_SHUTDOWN = 7,
            RETRO_ENVIRONMENT_SET_PERFORMANCE_LEVEL = 8,
            RETRO_ENVIRONMENT_GET_SYSTEM_DIRECTORY = 9,
            RETRO_ENVIRONMENT_SET_PIXEL_FORMAT = 10,
            RETRO_ENVIRONMENT_SET_INPUT_DESCRIPTORS = 11,
            RETRO_ENVIRONMENT_SET_KEYBOARD_CALLBACK = 12,
            RETRO_ENVIRONMENT_SET_DISK_CONTROL_INTERFACE = 13,
            RETRO_ENVIRONMENT_GET_VARIABLE = 15,
            RETRO_ENVIRONMENT_SET_VARIABLES = 16,
            RETRO_ENVIRONMENT_GET_VARIABLE_UPDATE = 17,
            RETRO_ENVIRONMENT_SET_SUPPORT_NO_GAME = 18,
            RETRO_ENVIRONMENT_GET_LIBRETRO_PATH = 19,
            RETRO_ENVIRONMENT_SET_FRAME_TIME_CALLBACK = 21,
            RETRO_ENVIRONMENT_SET_AUDIO_CALLBACK = 22,
            RETRO_ENVIRONMENT_GET_RUMBLE_INTERFACE = 23,
            RETRO_ENVIRONMENT_GET_INPUT_DEVICE_CAPABILITIES = 24,
            RETRO_ENVIRONMENT_GET_SENSOR_INTERFACE = (25 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_GET_CAMERA_INTERFACE = (26 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_GET_LOG_INTERFACE = 27,
            RETRO_ENVIRONMENT_GET_PERF_INTERFACE = 28,
            RETRO_ENVIRONMENT_GET_LOCATION_INTERFACE = 29,
            RETRO_ENVIRONMENT_GET_CONTENT_DIRECTORY = 30,
            RETRO_ENVIRONMENT_GET_CORE_ASSETS_DIRECTORY = 30,
            RETRO_ENVIRONMENT_GET_SAVE_DIRECTORY = 31,
            RETRO_ENVIRONMENT_SET_SYSTEM_AV_INFO = 32,
            RETRO_ENVIRONMENT_SET_PROC_ADDRESS_CALLBACK = 33,
            RETRO_ENVIRONMENT_SET_SUBSYSTEM_INFO = 34,
            RETRO_ENVIRONMENT_SET_CONTROLLER_INFO = 35,
            RETRO_ENVIRONMENT_SET_MEMORY_MAPS = (36 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_SET_GEOMETRY = 37,
            RETRO_ENVIRONMENT_GET_USERNAME = 38,
            RETRO_ENVIRONMENT_GET_LANGUAGE = 39,
            RETRO_ENVIRONMENT_GET_CURRENT_SOFTWARE_FRAMEBUFFER = (40 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_GET_HW_RENDER_INTERFACE = (41 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_SET_SUPPORT_ACHIEVEMENTS = (42 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_SET_HW_RENDER_CONTEXT_NEGOTIATION_INTERFACE = (43 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_SET_SERIALIZATION_QUIRKS = 44,
            RETRO_ENVIRONMENT_SET_HW_SHARED_CONTEXT = (44 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_GET_VFS_INTERFACE = (45 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_GET_LED_INTERFACE = (46 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_GET_AUDIO_VIDEO_ENABLE = (47 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_GET_MIDI_INTERFACE = (48 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_GET_FASTFORWARDING = (49 | RETRO_ENVIRONMENT_EXPERIMENTAL),
            RETRO_ENVIRONMENT_EXPERIMENTAL = 0x10000
        }

        public enum RetroPixelFormat
        {
            // 0RGB1555, native endian. 0 bit must be set to 0.
            // This pixel format is default for compatibility concerns only.
            // If a 15/16-bit pixel format is desired, consider using RGB565.
            RETRO_PIXEL_FORMAT_0RGB1555 = 0,

            // XRGB8888, native endian. X bits are ignored.
            RETRO_PIXEL_FORMAT_XRGB8888 = 1,

            // RGB565, native endian. This pixel format is the recommended format to use if a 15/16-bit format is desired
            // as it is the pixel format that is typically available on a wide range of low-power devices.
            // It is also natively supported in APIs like OpenGL ES.
            RETRO_PIXEL_FORMAT_RGB565 = 2,

            // Ensure sizeof() == sizeof(int).
            RETRO_PIXEL_FORMAT_UNKNOWN = int.MaxValue
        }
    }
}

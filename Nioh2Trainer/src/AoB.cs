namespace Nioh2Trainer.src
{
    static class AoB
    {
        public static byte[] LOCAL_PLAYER_BYTES
        {
            get
            {
                return new byte[] { 0x41, 0x8B, 0xC6, 0x48, 0x3B, 0x41, 0x20 };
            }
        }
        public static byte[] HEALTH_BYTES
        {
            get
            {
                return new byte[] { 0x8B, 0x43, 0x10, 0x2B, 0xC7 };
            }
        }
        public static byte[] STAMINA_BYTES
        {
            get
            {
                return new byte[] { 0xF3, 0x0F, 0x11, 0x59, 0x08 };
            }
        }
        public static byte[] ANIMA_BYTES
        {
            get
            {
                return new byte[] { 0x0F, 0x2F, 0xD9, 0xF3, 0x0F, 0x11, 0x11 };
            }
        }
        public static byte[] YONKAI_CHARGE_BYTES
        {
            get
            {
                return new byte[] { 0xF3, 0x0F, 0x10, 0x80, 0xC4, 0x00, 0x00, 0x00 };
            }
        }
        public static byte[] YONKAI_COOLDOWN_BYTES
        {
            get
            {
                return new byte[] { 0xF3, 0x41, 0x0F, 0x10, 0x80, 0xD0, 0x00, 0x00, 0x00 };
            }
        }
        public static byte[] YONKAI_TIME_BYTES
        {
            get
            {
                return new byte[] { 0xF3, 0x0F, 0x11, 0x49, 0x24 };
            }
        }
        public static byte[] BUFF_DURATION_BYTES
        {
            get
            {
                return new byte[] { 0xF3, 0x0F, 0x11, 0x43, 0x20 };
            }
        }
        public static byte[] ITEM_CONSUME_BYTES
        {
            get
            {
                return new byte[] { 0x66, 0x2B, 0xC5, 0x48, 0x8B, 0xCF };
            }
        }
        public static byte[] NINJUSTU_ONMYO_CONSUME_BYTES
        {
            get
            {
                return new byte[] { 0x66, 0x2B, 0xC7, 0x48, 0x8B, 0xCE };
            }
        }
        public static byte[] ITEM_PICK_UP_BYTES
        {
            get
            {
                return new byte[] { 0x66, 0x89, 0x51, 0x04, 0xC3 };
            }
        }
        public static byte[] GLORY_PICK_UP_BYTES
        {
            get
            {
                return new byte[] { 0x89, 0x87, 0xBC, 0x04, 0x00, 0x00 };
            }
        }
        public static byte[] AMRITA_PICK_UP_BYTES
        {
            get
            {
                return new byte[] { 0x48, 0x89, 0x87, 0x78, 0xB7, 0x07, 0x00 };
            }
        }
        public static byte[] PROFICIENCY_GAIN_BYTES
        {
            get
            {
                return new byte[] { 0x41, 0x03, 0x1C, 0x24, 0x3B, 0xD8 };
            }
        }
    }
}

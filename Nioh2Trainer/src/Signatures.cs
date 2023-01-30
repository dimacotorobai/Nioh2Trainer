namespace Nioh2Trainer
{
    static class Signatures
    {
        public static byte[] LOCALPLAYER
        {
            get
            {
                return new byte[] { 0x41, 0x8B, 0xC6, 0x48, 0x3B, 0x41, 0x20 };
            }
        }
        public static byte[] HEALTH
        {
            get
            {
                return new byte[] { 0x8B, 0x43, 0x10, 0x2B, 0xC7 };
            }
        }
        public static byte[] STAMINA
        {
            get
            {
                return new byte[] { 0xF3, 0x0F, 0x11, 0x59, 0x08, 0x7A };
            }
        }
        public static byte[] ANIMA
        {
            get
            {
                return new byte[] { 0x0F, 0x2F, 0xD9, 0xF3, 0x0F, 0x11, 0x11 };
            }
        }
        public static byte[] YONKAI_CHARGE
        {
            get
            {
                return new byte[] { 0xF3, 0x0F, 0x10, 0x80, 0xC4, 0x00, 0x00, 0x00, 0xF3, 0x0F, 0x2C };
            }
        }
        public static byte[] YONKAI_COOLDOWN
        {
            get
            {
                return new byte[] { 0xF3, 0x41, 0x0F, 0x10, 0x80, 0xD0, 0x00, 0x00, 0x00, 0x0F, 0x57 };
            }
        }
        public static byte[] YONKAI_TIME
        {
            get
            {
                return new byte[] { 0xF3, 0x0F, 0x11, 0x49, 0x24, 0xB0 };
            }
        }
        public static byte[] BUFF_DURATION
        {
            get
            {
                return new byte[] { 0xF3, 0x0F, 0x11, 0x43, 0x20, 0x76, 0x07, 0xF3 };
            }
        }
        public static byte[] ITEM_CONSUME
        {
            get
            {
                return new byte[] { 0x66, 0x2B, 0xC5, 0x48, 0x8B, 0xCF };
            }
        }
        public static byte[] NINJUSTU_ONMYO_CONSUME
        {
            get
            {
                return new byte[] { 0x66, 0x2B, 0xC7, 0x48, 0x8B, 0xCE };
            }
        }
        public static byte[] ITEM_PICK_UP
        {
            get
            {
                return new byte[] { 0x66, 0x89, 0x51, 0x04, 0xC3, 0xCC, 0x8B };
            }
        }
        public static byte[] GLORY_PICK_UP
        {
            /*
             * Address +1, for instuction(starts at 0x89)
             * Add +1 to returned signature start address
             */
            get
            {
                return new byte[] { 0xC1, 0x89, 0x87, 0xBC, 0x04, 0x00, 0x00 };
            }
        }
        public static byte[] AMRITA_PICK_UP
        {
            get
            {
                return new byte[] { 0x48, 0x89, 0x87, 0x78, 0xB7, 0x07, 0x00, 0x66, 0x41, 0x39, 0x6F, 0x0C, 0x74, 0x27, 0x49, 0x8B, 0xCF, 0xE8, 0x1A };
            }
        }
        public static byte[] PROFICIENCY_GAIN
        {
            get
            {
                return new byte[] { 0x41, 0x03, 0x1C, 0x24, 0x3B, 0xD8 };
            }
        }
    }
}
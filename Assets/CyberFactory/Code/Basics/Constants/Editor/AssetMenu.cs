namespace CyberFactory.Basics.Constants.Editor {

    public static class AssetMenu {
        
        private const string ROOT_NAME = "Cyber Factory/";
        private const int ROOT_ORDER = -1000;

        public static class Models {
            private const string NAME = "Models/";
            private const int ORDER = ROOT_ORDER + 0;

            public const string PLANT = NAME + "Plant";
            public const string PRODUCT = NAME + "Product";

            public const int PLANT_ORDER = ORDER + 0;
            public const int PRODUCT_ORDER = ORDER + 1;
        }
        
        public static class Systems {
            private const string NAME = "Systems/";
            private const int ORDER = ROOT_ORDER + 1;

            public const string PLANTS = NAME + "Plants/";
            public const string PRODUCTS = NAME + "Products/";
            public const string INVENTORY = NAME + "Inventory/";

            public const int PLANTS_ORDER = ORDER + 0;
            public const int PRODUCTS_ORDER = ORDER + 1;
            public const int INVENTORY_ORDER = ORDER + 2;
        }
        
    }
}
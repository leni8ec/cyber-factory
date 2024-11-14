namespace CyberFactory.Basics.Constants.Editor {

    public static class AssetMenu {

        private const int ROOT_ORDER = -1000;
        private const string ROOT_NAME = "Cyber Factory/";

        private const string PROVIDERS_NAME = ROOT_NAME; // "Add Component" -> "Cyber Factory > Plants > Plant"
        private const string SYSTEMS_NAME = "Systems/";
        private const string MODELS_NAME = "Models/";
        private const string CONFIGS_NAME = "Configs/";


        private const int COMMON_ORDER = ROOT_ORDER + 0;
        private const int PLANTS_ORDER = ROOT_ORDER + 1;
        private const int PRODUCTS_ORDER = ROOT_ORDER + 2;
        private const int INVENTORY_ORDER = ROOT_ORDER + 3;
        private const int UTILITY_ORDER = ROOT_ORDER + 9;

        private const int VIEW_ORDER_SHIFT = +100;


        private const string COMMON_SUFFIX = "Common/";
        private const string PLANTS_SUFFIX = "Plants/";
        private const string PRODUCTS_SUFFIX = "Products/";
        private const string INVENTORY_SUFFIX = "Inventory/";
        private const string UTILITY_SUFFIX = "Utility/";

        public static class Common {
            public const int ORDER = COMMON_ORDER;
            private const string SUFFIX = COMMON_SUFFIX;

            public const string PROVIDER = SYSTEMS_NAME + SUFFIX;
            public const string SYSTEM = SYSTEMS_NAME + SUFFIX;
            public const string MODEL = SYSTEMS_NAME + SUFFIX;
            public const string CONFIG = CONFIGS_NAME + SUFFIX;
        }

        public static class Plants {
            public const int ORDER = PLANTS_ORDER;
            public const int ORDER_VIEW = PLANTS_ORDER + VIEW_ORDER_SHIFT;
            public const string SUFFIX = PLANTS_SUFFIX;

            public const string PROVIDER = PROVIDERS_NAME + SUFFIX;
            public const string SYSTEM = SYSTEMS_NAME + SUFFIX;
            public const string MODEL = MODELS_NAME + "Plant"; // + SUFFIX;
            public const string CONFIG = CONFIGS_NAME + SUFFIX;
        }

        public static class Products {
            public const int ORDER = PRODUCTS_ORDER;
            public const int ORDER_VIEW = PLANTS_ORDER + VIEW_ORDER_SHIFT;
            public const string SUFFIX = PRODUCTS_SUFFIX;

            public const string PROVIDER = PROVIDERS_NAME + SUFFIX;
            public const string SYSTEM = SYSTEMS_NAME + SUFFIX;
            public const string MODEL = MODELS_NAME + "Product";
            public const string CONFIG = CONFIGS_NAME + SUFFIX;
        }

        public static class Inventory {
            public const int ORDER = INVENTORY_ORDER;
            public const string SUFFIX = INVENTORY_SUFFIX;

            public const string PROVIDER = PROVIDERS_NAME + SUFFIX;
            public const string SYSTEM = SYSTEMS_NAME + SUFFIX;
            public const string MODEL = MODELS_NAME + SUFFIX;
        }

        public static class Utility {
            public const int ORDER = UTILITY_ORDER;
            public const string SUFFIX = UTILITY_SUFFIX;

            public const string PROVIDER = PROVIDERS_NAME + SUFFIX;
            public const string SYSTEM = SYSTEMS_NAME + SUFFIX;
            public const string MODEL = MODELS_NAME + SUFFIX;
        }


        //
        // /// <summary>
        // /// Example: "Models/Plant"
        // /// </summary>
        // public static class Models {
        //     private const string NAME = "Models/";
        //     private const int ORDER = ROOT_ORDER + 0;
        //
        //     public const string PLANT = NAME + "Plant";
        //     public const string PRODUCT = NAME + "Product";
        //
        //     public const int PLANT_ORDER = ORDER + 0;
        //     public const int PRODUCT_ORDER = ORDER + 1;
        // }
        //
        // /// <summary>
        // /// Example: "Systems/Plants/Production" 
        // /// </summary>
        // public static class Systems {
        //     private const string NAME = "Systems/";
        //     private const int ORDER = ROOT_ORDER + 1;
        //
        //     public const string COMMON = NAME + "Common/";
        //     public const string PLANTS = NAME + "Plants/";
        //     public const string PRODUCTS = NAME + "Products/";
        //     public const string INVENTORY = NAME + "Inventory/";
        //     public const string UTILITY = NAME + "Utility/";
        //
        //     public const int COMMON_ORDER = ORDER + 0;
        //     public const int PLANTS_ORDER = ORDER + 1;
        //     public const int PRODUCTS_ORDER = ORDER + 2;
        //     public const int INVENTORY_ORDER = ORDER + 3;
        //     public const int UTILITY_ORDER = ORDER + 9;
        // }

    }
}
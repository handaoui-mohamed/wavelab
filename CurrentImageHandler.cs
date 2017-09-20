using ImageFunctions;

namespace Wave_Lab
{
    public class CurrentImageHandler : ImageHandler
    {
        public BrightnessHandler CurrentBrightnessHandler { get; set; }
        public ContrastHandler CurrentContrastHandler { get; set; }
        public CropHandler CurrentCropHandler { get; set; }
        public FilterHandler CurrentFilterHandler { get; set; }
        public GrayscaleHandler CurrentGrayscaleHandler { get; set; }
        public ImageFileHandler CurrentFileHandler { get; set; }
        public ImageInsertionHandler CurrentImgInsHandler { get; set; }
        public InversionHandler CurrentInvHandler { get; set; }
        public RotationHandler CurrentRotationHandler { get; set; }
        public SepiaToneHandler CurrentSepiaToneHandler { get; set; }
        public ShapeInsertionHandler CurrentShapeInsHandler { get; set; }
        public TextInsertionHandler CurrentTextInsHandler { get; set; }

        public CurrentImageHandler() : base()
        {
            CurrentBrightnessHandler = new BrightnessHandler(this);
            CurrentContrastHandler = new ContrastHandler(this);
            CurrentCropHandler = new CropHandler(this);
            CurrentFilterHandler = new FilterHandler(this);
            CurrentGrayscaleHandler = new GrayscaleHandler(this);
            CurrentFileHandler = new ImageFileHandler(this);
            CurrentImgInsHandler = new ImageInsertionHandler(this);
            CurrentInvHandler = new InversionHandler(this);
            CurrentRotationHandler = new RotationHandler(this);
            CurrentSepiaToneHandler = new SepiaToneHandler(this);
            CurrentShapeInsHandler = new ShapeInsertionHandler(this);
            CurrentTextInsHandler = new TextInsertionHandler(this);
        }
    }
}

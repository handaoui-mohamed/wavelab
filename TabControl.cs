using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Wave_Lab
{
    class TabControl
    {
        private Bitmap bitmap;
        private Stack<Bitmap> pile_undo;
        private Stack<Bitmap> pile_redo;
        private String information;

        public TabControl(Bitmap bitmap,Stack<Bitmap> pile_undo,Stack<Bitmap> pile_redo,String information)
        {
            this.bitmap = bitmap;
            this.pile_undo = new Stack<Bitmap>();
            this.pile_redo = new Stack<Bitmap>();
            this.pile_undo = pile_undo;
            this.pile_redo = pile_redo;
            this.information = information;
        }

        public TabControl()
        {

        }

        public Bitmap getBitmap()
        {
            return bitmap;
        }

        public void setBitmap(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public Stack<Bitmap> getPile_undo()
        {
            return pile_undo;
        }

        public void setPile_undo(Stack<Bitmap> pile_undo)
        {
            this.pile_undo = pile_undo;
        }

        public Stack<Bitmap> getPile_redo()
        {
            return pile_redo;
        }

        public void setPile_redo(Stack<Bitmap> pile_redo)
        {
            this.pile_redo = pile_redo;
        }

        public String getInformation()
        {
            return information;
        }

        public void setInformation(String information)
        {
            this.information = information;
        }
    }
}

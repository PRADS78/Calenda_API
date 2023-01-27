
using DisprzTraining.Model;

namespace DisprzTraining.CustomException
{
    public class InputTimeErrorException:ApplicationException
    {
        public ErrorResponse InputTimeError{get;private set;}
        public InputTimeErrorException(ErrorResponse error)
        {
            this.InputTimeError=error;
        }
    }
}
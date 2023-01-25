
using DisprzTraining.Model;

namespace DisprzTraining.CustomException
{
    public class InputErrorException:ApplicationException
    {
        public ErrorResponse InputError{get;private set;}
        public InputErrorException(ErrorResponse error)
        {
            this.InputError=error;
        }
    }
}
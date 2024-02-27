namespace UnityEngine
{
    public sealed class DataBindingBaseUsage : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                var dataSource = GameObject.FindObjectOfType<DataSource>();
                dataSource.Initialize(new EditorTestDataContext());
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                var dataSource = GameObject.FindObjectOfType<DataSource>();
                string value = Time.realtimeSinceStartup.ToString("F2");
                (dataSource.DataContext as EditorTestDataContext).Address.School.Street = value;
            }
        }
    }
}
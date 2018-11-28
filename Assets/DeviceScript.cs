using System.Collections.Generic;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;

public class DeviceScript : MonoBehaviour
{
    #region Constructors

    public DeviceScript()
    {
        this.summaries = new List<string>();
        this.summaries.Add("Audio Issue");
        this.summaries.Add("Graphics Issue");
        this.summaries.Add("Physics Issue");
    }

    #endregion

    #region Fields

    private bool isSubmitting;

    public RenderTexture RenderTexture;

    public TextMesh StatusText;

    private List<string> summaries;

    private int summaryIndex;

    public TextMesh SummaryText;

    #endregion

    #region Methods

    public void OnSubmit()
    {
        if (this.isSubmitting)
        {
            return;
        }
        this.isSubmitting = true;
        UnityUserReporting.CurrentClient.TakeScreenshotFromSource(2048, 2048, this.RenderTexture, s => { });
        UnityUserReporting.CurrentClient.TakeScreenshotFromSource(512, 512, this.RenderTexture, s => { });
        UnityUserReporting.CurrentClient.CreateUserReport((br) =>
        {
            UnityUserReporting.CurrentClient.SendUserReport(br, (success, br2) =>
            {
                this.isSubmitting = false;
            });
        });
    }

    public void OnSummaryChange()
    {
        this.summaryIndex++;
        if (this.summaryIndex >= this.summaries.Count)
        {
            this.summaryIndex = 0;
        }
    }

    private void Update()
    {
        if (this.SummaryText != null)
        {
            this.SummaryText.text = this.summaries[this.summaryIndex];
        }

        if (this.StatusText != null)
        {
            this.StatusText.text = this.isSubmitting ? "Sending" : "Ready";
        }
    }

    #endregion
}
﻿namespace Domain.Plan;

public class Limits(double maxUploadSize, double maxStorageSize, int maxEmails, bool canUseExpiresOnDownload, bool canUseQuickDownload, bool canUsePassword, int maxExpireDays)
{
    public double MaxUploadSize { get; } = maxUploadSize;
    public double MaxStorageSize { get; } = maxStorageSize;
    public int MaxEmails { get; } = maxEmails;
    public bool CanUseExpiresOnDownload { get; } = canUseExpiresOnDownload;
    public bool CanUseQuickDownload { get; } = canUseQuickDownload;
    public bool CanUsePassword { get; } = canUsePassword;
    public int MaxExpireDays { get; } = maxExpireDays;


}

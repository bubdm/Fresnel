﻿/****************************************************************************
  Generated by T4TS.tt - don't make any changes in this file
****************************************************************************/

declare module FresnelApp {
    /** Generated from Envivo.Fresnel.UiCore.Commands.BaseCommandResponse **/
    export interface BaseCommandResponse {
        Passed: boolean;
        Failed: boolean;
        Messages: any;
        Modifications: FresnelApp.ModificationsVM;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.CollectionAddNewRequest **/
    export interface CollectionAddNewRequest {
        CollectionID: any;
        ElementTypeName: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.CollectionAddRequest **/
    export interface CollectionAddRequest {
        CollectionID: any;
        ElementIDs: any[];
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.CollectionAddResponse **/
    export interface CollectionAddResponse extends FresnelApp.BaseCommandResponse {
        AddedItem: FresnelApp.ObjectVM;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.CollectionRemoveRequest **/
    export interface CollectionRemoveRequest {
        CollectionID: any;
        ElementID: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.CreateAndSetPropertyRequest **/
    export interface CreateAndSetPropertyRequest {
        ObjectID: any;
        PropertyName: string;
        ClassTypeName: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.CreateAndSetPropertyResponse **/
    export interface CreateAndSetPropertyResponse extends FresnelApp.BaseCommandResponse {
        NewObject: FresnelApp.ObjectVM;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.CreateCommandResponse **/
    export interface CreateCommandResponse extends FresnelApp.BaseCommandResponse {
        NewObject: FresnelApp.ObjectVM;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.GenericResponse **/
    export interface GenericResponse extends FresnelApp.BaseCommandResponse {
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.GetObjectRequest **/
    export interface GetObjectRequest {
        ObjectID: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.GetPropertyRequest **/
    export interface GetPropertyRequest {
        ObjectID: any;
        PropertyName: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.GetPropertyResponse **/
    export interface GetPropertyResponse extends FresnelApp.BaseCommandResponse {
        ReturnValue: FresnelApp.ObjectVM;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.InvokeMethodRequest **/
    export interface InvokeMethodRequest {
        ObjectID: any;
        MethodName: string;
        Parameters: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.InvokeMethodResponse **/
    export interface InvokeMethodResponse extends FresnelApp.BaseCommandResponse {
        ResultObject: FresnelApp.ObjectVM;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.SaveChangesRequest **/
    export interface SaveChangesRequest {
        ObjectID: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.SearchObjectsRequest **/
    export interface SearchObjectsRequest extends FresnelApp.SearchRequest {
        SearchType: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.SearchParameterRequest **/
    export interface SearchParameterRequest extends FresnelApp.SearchRequest {
        ObjectID: any;
        MethodName: string;
        ParameterName: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.SearchPropertyRequest **/
    export interface SearchPropertyRequest extends FresnelApp.SearchRequest {
        ObjectID: any;
        PropertyName: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.SearchRequest **/
    export interface SearchRequest {
        SearchFilters: any;
        OrderBy: string;
        IsDescendingOrder: boolean;
        PageSize: number;
        PageNumber: number;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.SearchResponse **/
    export interface SearchResponse extends FresnelApp.BaseCommandResponse {
        Result: FresnelApp.SearchResultsVM;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.SetParameterRequest **/
    export interface SetParameterRequest {
        ObjectID: any;
        MethodName: string;
        ParameterName: string;
        NonReferenceValue: any;
        ReferenceValueId: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Commands.SetPropertyRequest **/
    export interface SetPropertyRequest {
        ObjectID: any;
        PropertyName: string;
        NonReferenceValue: any;
        ReferenceValueId: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.BaseViewModel **/
    export interface BaseViewModel {
        IsVisible: boolean;
        IsEnabled: boolean;
        Name: string;
        Description: string;
        Error: string;
        Tooltip: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.Changes.CollectionElementVM **/
    export interface CollectionElementVM {
        CollectionId: any;
        ElementId: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.Changes.ModificationsVM **/
    export interface ModificationsVM {
        NewObjects: any;
        PropertyChanges: any;
        ObjectTitleChanges: any;
        CollectionAdditions: any;
        CollectionRemovals: any;
        MethodParameterChanges: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.Changes.ObjectTitleChangeVM **/
    export interface ObjectTitleChangeVM {
        ObjectId: any;
        Title: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.Changes.ParameterChangeVM **/
    export interface ParameterChangeVM {
        ObjectId: any;
        MethodName: string;
        ParameterName: string;
        State: FresnelApp.ValueStateVM;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.Changes.PropertyChangeVM **/
    export interface PropertyChangeVM {
        ObjectId: any;
        PropertyName: string;
        State: FresnelApp.ValueStateVM;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.Classes.ClassItem **/
    export interface ClassItem extends FresnelApp.BaseViewModel {
        Type: string;
        FullTypeName: string;
        Create: FresnelApp.InteractionPoint;
        Search: FresnelApp.InteractionPoint;
        RepositoryCommands: FresnelApp.InteractionPoint[];
        StaticMethodCommands: FresnelApp.InteractionPoint[];
        FactoryCommands: FresnelApp.InteractionPoint[];
        ServiceCommands: FresnelApp.InteractionPoint[];
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.Classes.Namespace **/
    export interface Namespace extends FresnelApp.BaseViewModel {
        FullName: string;
        Classes: FresnelApp.ClassItem[];
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.CollectionVM **/
    export interface CollectionVM extends FresnelApp.ObjectVM {
        IsCollection: boolean;
        ElementType: string;
        ElementProperties: any;
        Items: any;
        DisplayItems: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.InteractionPoint **/
    export interface InteractionPoint extends FresnelApp.BaseViewModel {
        CommandUri: string;
        CommandArg: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.MessageVM **/
    export interface MessageVM {
        OccurredAt: string;
        Text: string;
        Detail: string;
        RequiresAcknowledgement: boolean;
        IsSuccess: boolean;
        IsInfo: boolean;
        IsWarning: boolean;
        IsError: boolean;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.MethodVM **/
    export interface MethodVM extends FresnelApp.BaseViewModel {
        ObjectID?: any;
        Index: number;
        InternalName: string;
        Parameters: any;
        ParametersSetByUser: any;
        IsAsync: boolean;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.ObjectVM **/
    export interface ObjectVM extends FresnelApp.BaseViewModel {
        ID: any;
        Type: string;
        Properties: any;
        Methods: any;
        IsTransient: boolean;
        IsPersistent: boolean;
        IsModified: boolean;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.ParameterVM **/
    export interface ParameterVM extends FresnelApp.SettableMemberVM {
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.PropertyVM **/
    export interface PropertyVM extends FresnelApp.SettableMemberVM {
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.SearchResultItemVM **/
    export interface SearchResultItemVM extends FresnelApp.ObjectVM {
        IsSelected: boolean;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.SearchResultsVM **/
    export interface SearchResultsVM extends FresnelApp.CollectionVM {
        IsSearchResults: boolean;
        OriginalRequest: FresnelApp.SearchRequest;
        AreMoreAvailable: boolean;
        AllowSelection: boolean;
        AllowMultiSelect: boolean;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.SessionVM **/
    export interface SessionVM {
        UserName: string;
        LogonTime: string;
        Messages: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.SettableMemberVM **/
    export interface SettableMemberVM extends FresnelApp.BaseViewModel {
        ObjectID?: any;
        Index: number;
        InternalName: string;
        AllowedClassTypes: string[];
        IsRequired: boolean;
        IsLoaded: boolean;
        IsObject: boolean;
        IsCollection: boolean;
        IsNonReference: boolean;
        Info: any;
        State: FresnelApp.ValueStateVM;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.TypeInfo.BooleanVM **/
    export interface BooleanVM {
        Name: string;
        PreferredControl: any;
        IsNullable: boolean;
        TrueValue: string;
        FalseValue: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.TypeInfo.DateTimeVM **/
    export interface DateTimeVM {
        Name: string;
        PreferredControl: any;
        CustomFormat: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.TypeInfo.EnumItemVM **/
    export interface EnumItemVM extends FresnelApp.BaseViewModel {
        EnumName: string;
        Value: number;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.TypeInfo.EnumVM **/
    export interface EnumVM {
        Name: string;
        IsBitwiseEnum: boolean;
        Items: any;
        PreferredControl: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.TypeInfo.NullVM **/
    export interface NullVM {
        Name: string;
        PreferredControl: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.TypeInfo.NumberVM **/
    export interface NumberVM {
        Name: string;
        PreferredControl: any;
        MinValue: number;
        MaxValue: number;
        DecimalPlaces: number;
        CurrencySymbol: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.TypeInfo.ReferenceTypeVM **/
    export interface ReferenceTypeVM {
        FullTypeName: string;
        PreferredControl: any;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.TypeInfo.StringVM **/
    export interface StringVM {
        Name: string;
        PreferredControl: any;
        MinLength: number;
        MaxLength: number;
        EditMask: string;
    }
    /** Generated from Envivo.Fresnel.UiCore.Model.ValueStateVM **/
    export interface ValueStateVM {
        Value: any;
        ReferenceValueID?: any;
        FriendlyValue: string;
        ValueType: string;
        Get: FresnelApp.InteractionPoint;
        Set: FresnelApp.InteractionPoint;
        Create: FresnelApp.InteractionPoint;
        Clear: FresnelApp.InteractionPoint;
        Add: FresnelApp.InteractionPoint;
    }
}

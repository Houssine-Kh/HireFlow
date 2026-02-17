export interface JobDto{
    id : string,
    recruiterId : string,
    title : string,
    description : string,
    workMode : string,
    status : string   // 'Draft', 'Published', 'Closed'
}

export interface CreateJobRequest{
    title : string,
    description? : string,
    workMode? : string
}

export interface updateJobRequest{
    title : string,
    description? : string,
    workMode? : string
}
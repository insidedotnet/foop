namespace dependency

module OrderPlacingWorkflow =

    // used for placeholders
    type Undefined = exn  
    type ProductCode = Undefined

    //>Workflow1a
    type CheckProductCodeExists = 
        ProductCode -> bool
        // ^input      ^output
    //<

    type Address = Undefined
    type UnvalidatedAddress = Undefined

    //>Workflow1b2
    type CheckedAddress = CheckedAddress of UnvalidatedAddress
    //<

    //>Workflow1b
    type AddressValidationError = AddressValidationError of string

    type CheckAddressExists = 
        UnvalidatedAddress -> Result<CheckedAddress,AddressValidationError>
        // ^input                    ^output
    //<

    type UnvalidatedOrder = Undefined
    type ValidatedOrder = Undefined
    type ValidationError = Undefined

    //>Workflow1c
    type ValidateOrder = 
        CheckProductCodeExists    // dependency
          -> CheckAddressExists   // dependency
          -> UnvalidatedOrder     // input
          -> Result<ValidatedOrder,ValidationError>  // output